using System.Buffers;
using System.Diagnostics;
using System.Globalization;
using System.Text;

namespace Yaml.NET;

public sealed class YamlSerializer
{
    public static string Serialize(Node node)
    {
        var writer = new Utf8YamlWriter(new ArrayBufferWriter<byte>());
        Write(writer, node, indent: 0, hasDash: false);
        return Encoding.UTF8.GetString(writer.WrittenSpan);
    }

    private static void Write(Utf8YamlWriter writer, string key, Node node, int indent, bool hasDash)
    {
        writer.WriteIndentation(indent);
        writer.WriteKey(key);
        writer.WriteColon();

        if (node is ScalarNode scalar)
        {
            writer.WriteValue(scalar.Value);
            writer.WriteNewLine();
        }
        else
        {
            writer.WriteNewLine();
            indent += 2;
            Write(writer, node, indent, hasDash: false);
        }
    }

    private static void Write(Utf8YamlWriter writer, Node node, int indent, bool hasDash)
    {
        switch (node)
        {
            case ScalarNode scalar:
                Write(writer, scalar, indent, hasDash);
                break;
            case SequenceNode sequence:
                Write(writer, sequence, indent, hasDash);
                break;
            case MappingNode mapping:
                Write(writer, mapping, indent, hasDash);
                break;
            default:
                throw new InvalidOperationException("Invalid node");
        }
    }

    private static void Write(Utf8YamlWriter writer, ScalarNode node, int indent, bool hasDash)
    {
        writer.WriteIndentation(indent);

        if (hasDash)
        {
            writer.WriteDash();
        }

        writer.WriteValue(node.Value);

        writer.WriteNewLine();
    }

    private static void Write(Utf8YamlWriter writer, SequenceNode node, int indent, bool hasDash)
    {
        if (node.Count == 0)
            return;

        if (hasDash)
        {
            writer.WriteDashNewLine();
            indent += 2;
        }

        foreach (ref readonly var child in node.AsSpan())
            Write(writer, child, indent, hasDash: true);
    }

    private static void Write(Utf8YamlWriter writer, MappingNode node, int indent, bool hasDash)
    {
        if (node.Count == 0)
            return;

        if (hasDash)
        {
            writer.WriteDashNewLine();
            indent += 2;
        }

        foreach (var (key, child) in node)
            Write(writer, key, child, indent, hasDash: false);
    }
}

internal readonly ref struct Utf8YamlWriter
{
    private readonly ArrayBufferWriter<byte> _writer;

    public ReadOnlySpan<byte> WrittenSpan { get => _writer.WrittenSpan; }

    public Utf8YamlWriter(ArrayBufferWriter<byte> writer)
    {
        _writer = writer;
    }

    public void WriteNewLine()
    {
        // TODO: Can we define this during initialization?
        _writer.Write(Environment.NewLine.Length == 1 ? "\n"u8 : "\r\n"u8);
        Debug.WriteLine($"-----\n{Encoding.UTF8.GetString(_writer.WrittenSpan)}");
    }

    public void WriteIndentation(int indent)
    {
        if (indent <= 0)
            return;

        const byte space = (byte)' ';
        var whiteSpace = _writer.GetSpan(indent);
        whiteSpace[..indent].Fill(space);
        _writer.Advance(indent);
    }

    public void WriteDash()
    {
        _writer.Write("- "u8);
    }

    public void WriteDashNewLine()
    {
        _writer.Write("-"u8);
        WriteNewLine();
    }

    public void WriteColon()
    {
        _writer.Write(": "u8);
    }

    public void WriteKey(ReadOnlySpan<char> value)
    {
        if (value.Length == 0)
            return;

        var bufferLength = Encoding.UTF8.GetByteCount(value);

        var buffer = _writer.GetSpan(bufferLength)[..bufferLength];

        if (Encoding.UTF8.GetBytes(value, buffer) != bufferLength)
            throw new InvalidOperationException($"Unable to encode scalar '{value}'");

        _writer.Advance(bufferLength);
    }

    public void WriteValue(object? value)
    {
        switch (value)
        {
            case null:
                WriteNull();
                break;

            case bool b:
                WriteBoolean(b);
                break;

            case long i:
                WriteInt64(i);
                break;

            case double f:
                WriteFloat64(f);
                break;

            case string s:
                WriteString(s);
                break;
            default:
                throw new InvalidOperationException($"Unexpected scalar '{value}' with type {value.GetType()}");
        }
    }

    public void WriteNull()
    {
        _writer.Write("null"u8);
    }

    public void WriteBoolean(bool value)
    {
        _writer.Write(value ? "true"u8 : "false"u8);
    }

    public void WriteString(ReadOnlySpan<char> value)
    {
        if (value.Length == 0)
            return;

        var bufferLength = Encoding.UTF8.GetByteCount(value);

        var bufferLengthWithQuotes = bufferLength + 2;

        var buffer = _writer.GetSpan(bufferLengthWithQuotes)[..bufferLengthWithQuotes];

        if (Encoding.UTF8.GetBytes(value, buffer[1..^1]) != bufferLength)
            throw new InvalidOperationException($"Unable to encode scalar '{value}'");

        buffer[0] = (byte)'"';
        buffer[^1] = (byte)'"';

        _writer.Advance(bufferLengthWithQuotes);
    }

    public void WriteInt64(long value)
    {
        WriteFormattable(value, maxBufferLength: 20);
    }

    public void WriteFloat64(double value)
    {
        WriteFormattable(value, maxBufferLength: 24);
    }

    private void WriteFormattable<T>(T value, int maxBufferLength) where T : ISpanFormattable
    {
        Span<char> charBuffer = stackalloc char[maxBufferLength];
        if (!value.TryFormat(charBuffer, out var charsWritten, format: default, provider: CultureInfo.InvariantCulture))
            throw new InvalidOperationException($"Unable to write scalar '{value}'");

        var buffer = _writer.GetSpan(charsWritten)[..charsWritten];
        if (Encoding.UTF8.GetBytes(charBuffer[..charsWritten], buffer) != charsWritten)
            throw new InvalidOperationException($"Unable to encode scalar '{value}'");

        _writer.Advance(charsWritten);
    }
}
