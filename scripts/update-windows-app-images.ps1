echo "Working on LockScreenLogo.scale-200.png..."
inkscape -o ../src/GpxViewer2.WindowsApp/Images/LockScreenLogo.scale-200.png -w 48 -h 48 ../assets/GPXviewer.svg

echo "Working on SplashScreen.scale-200.png..."
inkscape -o ../src/GpxViewer2.WindowsApp/Images/SplashScreen.scale-200-temp.png -w 440 -h 440 ../assets/GPXviewer.svg
Start-Sleep 5
magick ../src/GpxViewer2.WindowsApp/Images/SplashScreen.scale-200-temp.png -background transparent -gravity center -extent 1240x600 ../src/GpxViewer2.WindowsApp/Images/SplashScreen.scale-200.png
rm ../src/GpxViewer2.WindowsApp/Images/SplashScreen.scale-200-temp.png

echo "Working on Square44x44Logo.scale-200.png..."
inkscape -o ../src/GpxViewer2.WindowsApp/Images/Square44x44Logo.scale-200.png -w 88 -h 88 ../assets/GPXviewer.svg

echo "Working on Square44x44Logo.targetsize-24_altform-unplated.png..."
inkscape -o ../src/GpxViewer2.WindowsApp/Images/Square44x44Logo.targetsize-24_altform-unplated.png -w 24 -h 24 ../assets/GPXviewer.svg

echo "Working on Square150x150Logo.scale-200.png..."
inkscape -o ../src/GpxViewer2.WindowsApp/Images/Square150x150Logo.scale-200.png -w 300 -h 300 ../assets/GPXviewer.svg

echo "Working on StoreLogo.png..."
inkscape -o ../src/GpxViewer2.WindowsApp/Images/StoreLogo.png -w 50 -h 50 ../assets/GPXviewer.svg

echo "Working on Wide310x150Logo.scale-200.png"
inkscape -o ../src/GpxViewer2.WindowsApp/Images/Wide310x150Logo.scale-200-temp.png -w 220 -h 220 ../assets/GPXviewer.svg
Start-Sleep 5
magick ../src/GpxViewer2.WindowsApp/Images/Wide310x150Logo.scale-200-temp.png -background transparent -gravity center -extent 620x300 ../src/GpxViewer2.WindowsApp/Images/Wide310x150Logo.scale-200.png
rm ../src/GpxViewer2.WindowsApp/Images/Wide310x150Logo.scale-200-temp.png