using System.Collections;
using System.Runtime.InteropServices;

namespace Yaml.NET;

public sealed record class SequenceNode() : Node(NodeKind.Sequence, Tag.Sequence), IList<Node>
{
    private readonly List<Node> _children = new();

    public Node this[int index] { get => _children[index]; set => _children[index] = value; }

    public int Count { get => _children.Count; }
    bool ICollection<Node>.IsReadOnly { get => ((ICollection<Node>)_children).IsReadOnly; }

    public void Add(Node item) => _children.Add(item);
    public void Clear() => _children.Clear();
    public bool Contains(Node item) => _children.Contains(item);
    public void CopyTo(Node[] array, int arrayIndex) => _children.CopyTo(array, arrayIndex);
    public IEnumerator<Node> GetEnumerator() => _children.GetEnumerator();
    public int IndexOf(Node item) => _children.IndexOf(item);
    public void Insert(int index, Node item) => _children.Insert(index, item);
    public bool Remove(Node item) => _children.Remove(item);
    public void RemoveAt(int index) => _children.RemoveAt(index);
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    internal Span<Node> AsSpan() => CollectionsMarshal.AsSpan(_children);
}