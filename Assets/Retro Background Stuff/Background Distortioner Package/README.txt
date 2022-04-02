-----------INSTRUCTIONS-----------

Thanks for downloading this package!

Follow the instructions in order to use this properly, please.


** Importing the package **

Click on your assets folder and right click to select "Import Custom Package", this way it imports the files automatically.
Alternatively, use the Assets folder also included.


** Important Prerequisite **
Images used as Textures must have the Read/Write option enabled.

To do this, click on the image to open the Inspector. In it, make sure
the Texture Type is Default or the following option will not appear.

Finally, click on the Read/Write Enabled checkbox to enable it otherwise Unity will throw an error at runtime start. 

On your Main Camera, the best results are with Skybox set to a solid color and with a Perspective projecting setting.

Wrap Mode set to Repeat is also recommended.


** STEPS **

1. Create a Plane game object into your scene, or use the TemplateScene included in the package at the Scenes folder.

2. Open its child objects and do the following for each layer to configure one.

*NOTE* Alternatively, you can select one of the pre-defined presets for each layer in the "Select Preset" option
in the "Configure Background" component. Otherwise you can configure your own as follows:

3. In the Inspector, open up the Configure Background component to start the configuration.

	3a. Source Texture, assign a source image to manipulate.

	3b. Color Shift Gradient, create gradient for the palette cycle 	change. Changes during runtime will not be applied until restarting.
	
	3c. Color Shift, shift color palette cycling.

	3d. Type of Distortion, choose different kinds of distortion for 	effect.

	3e. Opacity, change opacity of layer self explanatory.

	3f. Speed, change speed of Color Palette Cycle.

	3g. Tile X and Tile Y, size of tiles to repeat in the surface.

	3h. Amplitude, change amplitude of offset change in vertex.

	3i. Frequency, change frequency of offset change in vertex.

	3j. Scale, change scale of every variable.

	3k. Line Width, for offset distortion types the width of intervals 	that divide the texture.

	3l. Bloom Effect, values bigger than 1 give a "Bloom Effect" use 	different values for experimentation.

4. After both of the layers are configured, run the program for the effect to run. Every value except the Color Gradient can be changed during runtime for convenience. While not at run-time the plane will not show the correct colors until runtime.

To test out this package use the 4 available Scenes with different effects.


If you have any trouble, feedback, or complains feel free to email me
at albertcastaned@gmail.com.
	
