﻿namespace CookedAssetSerializerGUI;

static class TreeViewExtensions
{
    public static IEnumerable<TreeNode> Children(this TreeNode node, bool fulltree = true)
    {
        foreach (TreeNode n in node.Nodes)
        {
            yield return n;

            if (fulltree)
            {
                foreach (TreeNode child in Children(n))
                    yield return child;
            }

        }
    }

    public static IEnumerable<TreeNode> Parents(this TreeNode node)
    {
        var p = node.Parent;

        while (p != null)
        {
            yield return p;

            p = p.Parent;
        }
    }

    public static List<string> GatherCheckedFiles(this TreeNode topnode)
    {
        var files = new List<string>();
        foreach (var node in topnode.Children())
        {
            if (node.Checked && node.Nodes.Count == 0)
            {
                var path = node.Tag.ToString() ?? ""; //just to be safe
                if (path.EndsWith("uasset"))
                {
                    files.Add(path);
                }
                else
                {
                    files.AddRange(Directory.GetFiles(path, "*.uasset", SearchOption.AllDirectories));
                }
            }

        }
        return files;
    }

    public static void Refresh(this TreeNode node)
    {
        //TO-DO
        //Scan for changes without clearing full tree below

        //var temp = GatherCheckedFiles(node);
        
    }
}