import java.awt.*;
import java.awt.image.*;

// Put the right screen size in here
int screenW     = 1366;
int screenH     = 768;

// Preview window size
int windowW    = 100;
int windowH    = 100;

// Define the box where we want to capture and analyse colour from
// The start point of the box (top and left coordinate)
int boxL   = 633; // That's the middle of the screen
int boxT   = 334;
// Size of the box 
int boxW    = 100;
int boxH    = 100;

//How many pixels to skip while reading (the more you skip, it runs faster, but result might get worse)
int pixelSkip  = 2;

// Screen Area to be captured (usually the whole screen)
Rectangle dispBounds;
// creates object from java library that lets us take screenshots
Robot bot;

void setup(){

  // Create screenshot area
  dispBounds = new Rectangle(new Dimension(screenW,screenH));
  // Create Preview Window
  size(100, 100);

  // Standard Robot class error check and Create screenshot Robot
  try   {
    bot = new Robot();
  }
  catch (AWTException e)  {
    println("Robot class not supported by your system!");
    exit();
  }
}

void draw(){

  //ARGB value of a pixel, contains sets of 8 bytes values for Alpha, Red, Green, Blue
  int pixel;

  int r = 0;
  int g = 0;
  int b = 0;

  // Take screenshot
  BufferedImage screenshot = bot.createScreenCapture(dispBounds);

  // Pass all the ARGB values of every pixel into an array
  int[] screenData = ((DataBufferInt)screenshot.getRaster().getDataBuffer()).getData();

  //Find the RGB values of the region we want
  for(int i = boxT; i < (boxT + boxH); i += pixelSkip){      
    for(int j = boxL; j < (boxL + boxW); j += pixelSkip){

                        pixel = screenData[ i*screenW + j ];              
                        r += 0xff & (pixel>>16);
      g += 0xff & (pixel>>8 );
      b += 0xff &  pixel;

    }
  }

  // take average RGB values.
  r  = r / (boxH/pixelSkip * boxW/pixelSkip);
  g  = g / (boxH/pixelSkip * boxW/pixelSkip);
  b  = b / (boxH/pixelSkip * boxW/pixelSkip);

  color rgb = color((short)r, (short)g, (short)b);
  fill(rgb);
  rect(0, 0, 100, 100);

  println(frameRate);
}