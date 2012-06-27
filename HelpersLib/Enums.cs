﻿#region License Information (GPL v3)

/*
    ShareX - A program that allows you to take screenshots and share any file type
    Copyright (C) 2012 ShareX Developers

    This program is free software; you can redistribute it and/or
    modify it under the terms of the GNU General Public License
    as published by the Free Software Foundation; either version 2
    of the License, or (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program; if not, write to the Free Software
    Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.

    Optionally you can also view the license at <http://www.gnu.org/licenses/>.
*/

#endregion License Information (GPL v3)

using System;
using System.ComponentModel;

namespace HelpersLib
{
    // http://en.wikipedia.org/wiki/List_of_file_formats

    public enum ImageFileExtensions
    {
        [Description("Joint Photographic Experts Group")]
        jpg, jpeg,

        [Description("Portable Network Graphic")]
        png,

        [Description("CompuServe's Graphics Interchange Format")]
        gif,

        [Description("Microsoft Windows Bitmap formatted image")]
        bmp,

        [Description("File format used for icons in Microsoft Windows")]
        ico,

        [Description("Tagged Image File Format")]
        tif, tiff
    }

    public enum TextFileExtensions
    {
        [Description("ASCII or Unicode plaintext")]
        txt, log,

        [Description("ASCII or extended ASCII text file")]
        nfo,

        [Description("C source")]
        c,

        [Description("C++ source")]
        cpp, cc, cxx,

        [Description("C/C++ header file")]
        h,

        [Description("C++ header file")]
        hpp, hxx,

        [Description("C# source")]
        cs,

        [Description("Visual Basic.NET source")]
        vb,

        [Description("HyperText Markup Language")]
        html, htm,

        [Description("eXtensible HyperText Markup Language")]
        xhtml, xht,

        [Description("eXtensible Markup Language")]
        xml,

        [Description("Cascading Style Sheets")]
        css,

        [Description("JavaScript and JScript")]
        js,

        [Description("Hypertext Preprocessor")]
        php,

        [Description("Batch file")]
        bat,

        [Description("Java source")]
        java,

        [Description("Lua")]
        lua,

        [Description("Python source")]
        py,

        [Description("Perl")]
        pl,

        [Description("Visual Studio solution")]
        sln
    }

    public enum VideoFileExtensions
    {
        [Description("MPEG-4 Video File")]
        mp4, m4v
    }

    public enum EncryptionStrength
    {
        Low = 128, Medium = 192, High = 256
    }

    public enum EDataType
    {
        Default, File, Image, Text, URL, SocialNetworkingServiceRequest
    }

    public enum EInputType
    {
        None, Clipboard, FileSystem, Screenshot
    }

    public enum GIFQuality
    {
        Default, Bit8, Bit4, Grayscale
    }

    public enum EImageFormat
    {
        PNG, JPEG, GIF, BMP, TIFF
    }

    public enum AnimatedImageFormat
    {
        PNG, GIF
    }

    public enum TaskStatus
    {
        InQueue, Preparing, Working, Completed
    }

    public enum TaskProgress
    {
        ReportStarted, ReportProgress
    }

    public enum WindowButtonAction
    {
        [Description("Minimize to Tray")]
        MinimizeToTray,

        [Description("Minimize to Taskbar")]
        MinimizeToTaskbar,

        [Description("Exit Application")]
        ExitApplication,

        [Description("Do Nothing")]
        Nothing
    }

    /// <summary>
    /// This Enum must not be restructured. New items must append at the end to avoid mapping to the wrong item.
    /// </summary>
    [Flags]
    [TypeConverter(typeof(EnumToStringUsingDescription))]
    public enum Subtask
    {
        None = 1 << 0,

        [Description("Annotate image"), Category(ComponentModelStrings.ActivitiesAfterCaptureEffects)]
        AnnotateImage = 1 << 1,

        [Description("Add torn paper effect"), Category(ComponentModelStrings.ActivitiesAfterCaptureEffects)]
        AnnotateImageAddTornEffect = 1 << 2,

        [Description("Add shadow border"), Category(ComponentModelStrings.ActivitiesAfterCaptureEffects)]
        AnnotateImageAddShadowBorder = 1 << 3,

        [Description("Add watermark"), Category(ComponentModelStrings.ActivitiesAfterCaptureEffects)]
        AddWatermark = 1 << 4,

        [Description("Open with Image Effects Studio"), Category(ComponentModelStrings.ActivitiesAfterCaptureEffects)]
        ShowImageEffectsStudio = 1 << 5,

        [Description("Copy image to clipboard"), Category(ComponentModelStrings.ActivitiesAfterCapture)]
        CopyImageToClipboard = 1 << 6,

        [Description("Save to file"), Category(ComponentModelStrings.ActivitiesAfterCapture)]
        SaveToFile = 1 << 7,

        [Description("Save to file with dialog"), Category(ComponentModelStrings.ActivitiesAfterCapture)]
        SaveImageToFileWithDialog = 1 << 8,

        [Description("Run external program"), Category(ComponentModelStrings.ActivitiesAfterCapture)]
        RunExternalProgram = 1 << 9,

        [Description("Upload to remote host"), Category(ComponentModelStrings.ActivitiesUploaders)]
        UploadToRemoteHost = 1 << 10,

        [Description("Shorten URL"), Category(ComponentModelStrings.ActivitiesAfterCapture)]
        ShortenUrl = 1 << 11,

        [Description("Send to printer"), Category(ComponentModelStrings.ActivitiesAfterCapture)]
        Print = 1 << 12,
    }

    [Flags]
    public enum AfterUploadTasks
    {
        None = 0,
        [Description("Use URL shortener")]
        UseURLShortener = 1,
        [Description("Post URL to social networking service")]
        ShareUsingSocialNetworkingService = 1 << 1,
        [Description("Copy URL to clipboard")]
        CopyURLToClipboard = 1 << 2
    }

    public enum HotkeyStatus
    {
        Registered, Failed, NotConfigured
    }

    public enum TriangleAngle
    {
        Top, Right, Bottom, Left
    }

    public enum HashType
    {
        MD5, SHA1, SHA256, SHA384, SHA512, RIPEMD160
    }

    public enum TokenType
    {
        Unknown,
        Whitespace,
        Symbol,
        Literal,
        Identifier,
        Numeric,
        Keyword
    }
}