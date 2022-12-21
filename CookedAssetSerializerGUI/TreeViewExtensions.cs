using System.Xml.Linq;

namespace CookedAssetSerializerGUI;

static class TreeViewExtensions
{
    private static TreeNode? ContentNode(this TreeView tree) => tree.Nodes.Count != 0 ? tree.Nodes[0] : null; 
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

    public static List<string> GatherAllFiles(this TreeNode topnode)
    {
        var files = new List<string>();
        foreach (var node in topnode.Children())
        {
            if (node.Checked && node.Nodes.Count == 0)
            {
                var path = node.Name ?? ""; //just to be safe
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

    public static List<string> GatherMinFileList(this TreeNode topnode)
    {
        var files = new List<string>();

        if (!topnode.Checked) return files;

        var savedir = true;
        foreach (var node in topnode.Children())
        {
            if (!node.Checked)
            {
                savedir = false;
                break;
            } 
        }

        if (savedir)
        {
            files.Add(topnode.Name);
            return files;
        }

        foreach (var node in topnode.Children(false))
        {
            files.AddRange(node.GatherMinFileList());
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