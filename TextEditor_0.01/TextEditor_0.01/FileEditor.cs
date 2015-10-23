using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScintillaNET;
using System.Windows.Forms;

public class FileEditor
{
    private string path;
    private string shortName;
    private TextBox txt;
    bool isDirty;
    bool isNew;

    public FileEditor()
    {
        isDirty = false;
        isNew = true;
        path = "";
        shortName = "untitled";
        txt = new TextBox(); //Create a new text box
        
    }

    public TextBox getTextBox()
    {
        return (txt);
    }

    public string TabLabel()
    {
        if (isDirty)
            return shortName + "*";
        else
            return shortName;
    }

    public bool IsDirty()
    {
        return isDirty;
    }

    public bool IsNew()
    {
        return isNew;
    }

    public void SetDirty(bool b)
    {
        isDirty = b;
    }

    public void SetNew(bool b)
    {
        isNew = b;
    }

    public void SetPath(string _path)
    {
        path = _path;
    }

    public string Path()
    {
        return path;
    }

    public string ShortName()
    {
        return shortName;
    }

    public String getText()
    {
        return (txt.Text);
    }

    public String[] getTextLines()
    {
        return (txt.Lines);
    }

    public void setText(String t)
    {
        txt.Text = t;
    }
}
