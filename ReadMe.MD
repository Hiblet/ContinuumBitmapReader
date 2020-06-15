# ContinuumBitmapReader

Alteryx C# plug-in to take in a path to an image file (BMP, JPEG, GIF, TIFF, PNG), and output the Red/Green/Blue colour values at each pixel, and the alpha value (transparency).

Provided free, courtesy of Continuum (www.continuum.je)



## Introduction

This project is a Windows Visual Studio 2017 project for .Net Framework 4.6.1.  It is a Class Library DLL for use with Alteryx, compiled to x64.

The app implements a Alteryx plug-in tool that processes graphic images (pictures) and shows the colour value at each pixel, and the alpha value.  

The colour value is expressed as three values for Red, Green and Blue.  Zero values for each represent black.   Maximum values (ie 255) for each represent white.  

The alpha value is the transparency, where 0 is transparent and 255 is opaque.



## Installation

The DLL must be copied to "C:\Program Files\Alteryx\bin\Plugins\ContinuumBitmapReader" on your local installation.  

Next, you need an INI file in "C:\Program Files\Alteryx\Settings\AdditionalPlugIns".  The INI file is included in this repo, so you can copy it to your local folder.  The INI file has the path info that Alteryx needs to find the DLL, and the ToolGroup that the tool should appear in, in this case "Developer".

If you are not worried about putting the tool in a particular group, you can just copy the DLL to "C:\Program Files\Alteryx\bin\Plugins".  Once the DLL is there, it will become available in the Alteryx Designer Search tool, as "BitmapReader".

The tool will have a grey-cog-wheel icon with a camera icon in the centre.


## Usage: Loading image files

Get the tool installed and check you can see it in Alteryx (see the Installation section above).

Get an image file that is one of the following formats:
 - BMP (Bitmap)
 - JPG or JPEG 
 - TIFF
 - GIF
 - PNG
 
Note the folder that your target image is in, because you will need to give the tool the location of the image file.

Set up a new Alteryx workflow and create a TextInput tool.  Create a Filename column, and put the full path to your image file, including drive letter and filename extension, as the value in the Filename column.

Add this tool, and connect it to the TextInput tool.

Configure the tool to accept the Filename column as input.  

The tool will try to load the file that you have specified in the Filename column.

If the file is not in one of the correct formats, the tool will bloop out an error message.

If the file is in an acceptable format, the tool will output one row for each pixel in the image (potentially a lot of data).

Each output row will have the X and Y coordinate, the Red, Green and Blue value at that pixel, and the Alpha value at that pixel.  The Alpha value is the transparency of the pixel.

Note that you can load multiple images with one instance of the tool, and each image can be a different format.

The file will be locked until the image has fully loaded, and then unlocked when the tool finishes loading that image file.



## Example

A couple of images like this...

![PacMan](./Images/pacman.png?raw=true "PacMan")

![Ghost](./Images/ghost.jpg?raw=tue "Ghost")

... will be loaded to this data:


 | Filename | XCoord | YCoord | RValue | GValue | BValue | AlphaValue | 
 | -------- | ------ | ------ | ------ | ------ | ------ | ---------- | 
 | pacman.png | 0 | 0 | 255 | 255 | 255 | 0 | 
 | pacman.png | 1 | 0 | 255 | 255 | 255 | 0 | 
 | pacman.png | 2 | 0 | 255 | 216 | 0 | 255 | 
 | pacman.png | 3 | 0 | 255 | 216 | 0 | 255 | 
 | pacman.png | 4 | 0 | 255 | 216 | 0 | 255 | 
 | pacman.png | 5 | 0 | 255 | 216 | 0 | 255 | 
 | pacman.png | 6 | 0 | 255 | 255 | 255 | 0 | 
 | pacman.png | 7 | 0 | 255 | 255 | 255 | 0 | 
 | pacman.png | 0 | 1 | 255 | 255 | 255 | 0 | 
 | pacman.png | 1 | 1 | 255 | 216 | 0 | 255 | 
 | pacman.png | 2 | 1 | 255 | 216 | 0 | 255 | 
 | pacman.png | 3 | 1 | 255 | 216 | 0 | 255 | 
 | pacman.png | 4 | 1 | 255 | 216 | 0 | 255 | 
 | pacman.png | 5 | 1 | 255 | 216 | 0 | 255 | 
 | pacman.png | 6 | 1 | 255 | 216 | 0 | 255 | 
 | pacman.png | 7 | 1 | 255 | 255 | 255 | 0 | 


Only first 16 rows shown; Full data example is in the repo, in AlteryxTests/Test05_Example.csv.

!(./AlteryxTests/Test05_Example.csv)


## Performance

Images can be large, so performance might be important to you.  As an approximate benchmark, I have successfully loaded a 27 Megapixel image to 27 million Alteryx rows in 3 mins, on my desktop PC, which is an Intel i7 (8th Gen) with 16Gb Ram and SSD hard drive.  


## Author

Steve Hibbert (steve at Continuum dot je), May 2020 (www.continuum.je)



## License

MIT License: Use as you see fit, give credit where applicable.


## Updates

### V 1.0.0
Alpha Release.



## Credits and Acknowledgements

This tool is surprisingly simple and uses C# Image and Bitmap classes to do all the funky stuff.
