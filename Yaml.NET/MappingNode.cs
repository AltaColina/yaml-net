using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace Yaml.NET;

public sealed record class MappingNode() : Node(NodeKind.Mapping, Tag.Mapping), IDictionary<string, Node>
{
    private readonly Dictionary<string, Node> _map = new();

    public Node this[string key] { get => _map[key]; set => _map[key] = value; }

    public ICollection<string> Keys { get => _map.Keys; }
    public ICollection<Node> Values { get => _map.Values; }
    public int Count { get => _map.Count; }
    bool ICollection<KeyValuePair<string, Node>>.IsReadOnly { get => ((ICollection<KeyValuePair<string, Node>>)_map).IsReadOnly; }

    public void Add(string key, Node value) => _map.Add(key, value);
    void ICollection<KeyValuePair<string, Node>>.Add(KeyValuePair<string, Node> item) => ((ICollection<KeyValuePair<string, Node>>)_map).Add(item);
    public void Clear() => _map.Clear();
    bool ICollection<KeyValuePair<string, Node>>.Contains(KeyValuePair<string, Node> item) => _map.Contains(item);
    public bool ContainsKey(string key) => _map.ContainsKey(key);
    void ICollection<KeyValuePair<string, Node>>.CopyTo(KeyValuePair<string, Node>[] array, int arrayIndex) => ((ICollection<KeyValuePair<string, Node>>)_map).CopyTo(array, arrayIndex);
    public IEnumerator<KeyValuePair<string, Node>> GetEnumerator() => _map.GetEnumerator();
    public bool Remove(string key) => _map.Remove(key);
    bool ICollection<KeyValuePair<string, Node>>.Remove(KeyValuePair<string, Node> item) => ((ICollection<KeyValuePair<string, Node>>)_map).Remove(item);
    public bool TryGetValue(string key, [MaybeNullWhen(false)] out Node value) => _map.TryGetValue(key, out value);
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_map).GetEnumerator();
}