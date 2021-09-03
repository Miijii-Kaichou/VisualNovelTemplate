>> The Use of Fonts and FontAssets for Visual Novel Template
---------------------------------------------

You can either use the default font that Unity provides for your fontface. Another way you could grab some fonts is by going into your fonts directory through C:/Windows/Fonts, and click and dragging those fonts into the Assets>VNSource>Fonts folder.

You could also go to online side with numerous of fontfaces just to meet your theme. One I honestly prefer is DaFont.com. You get a lot of fonts there (but be sure you check the licensing for those fonts).

Unity's Text UI mainly uses a package called TextMeshPro. Through the fonts you've found, in order to use them in the game, you have to convert them to FontAssets. Luckily, that's really easy. 

Head to Windows>TextMeshPro>Font Asset Creator. At the very top of the window that pops up, you'll find a label that says "Source Font File". That's where you drag all your fonts in to create your FontAssets. 

Keep Kerning Pairs on (because it'll be essential for any Visual Novel). Make sure your resolution is between 2048 to 4096. You could go higher, but that's mainly for fonts that includes either Chinese Characters or Japanese Kanji.

Either have the Packing Method Optimum or Fast. For nice, and not jaggedy fonts, set the Render Mode to any of the SMOOTH options. Don't worry about padding, and for the most part, you can keep you Sampling Point Size the same 128.

After you're set up, click on "Generate Font Atlas". Depending on your font, it's going to take awhile, but give it time. It's okay.

If you want ever Unicode there is provided in the font file, change the Character Set to "Unicode Range (Hex)". That'll also resolve the missing characters that you'll sometimes get.