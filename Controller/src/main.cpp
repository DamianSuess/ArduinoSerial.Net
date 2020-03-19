#include <Arduino.h>
#include <Adafruit_DotStar.h>

// Configure IDE <3
// https://www.youtube.com/watch?v=dany7ae_0ks

uint32_t _color = 0xFF0000; // Start with RED
Adafruit_DotStar _dotStar(1, INTERNAL_DS_DATA, INTERNAL_DS_CLK, DOTSTAR_BGR);

void setup()
{
  // Init digital pin 13 (LED) as an output
  pinMode(13, OUTPUT);

  // Serial monitoring
  Serial.begin(9600);

  // DotStar Initi
  _dotStar.begin();
  _dotStar.setBrightness(20);
  _dotStar.show(); // Turn off all LEDs
}

void DotStarColor(uint32_t color)
{
  _dotStar.setBrightness(20);
  _dotStar.setPixelColor(0, color);
  _dotStar.show();
}

void BlinkWithDelay(int msDelay)
{
  // Toggle LED
  digitalWrite(13, HIGH);
  delay(msDelay);
  digitalWrite(13, LOW);
  //delay(msDelay);
}

void CommTest()
{
    // Total of 400ms delay
  BlinkWithDelay(200);

  char _tx = '0';
  char rx;

  rx = Serial.read();
  switch (rx)
  {
    case '1':
      _tx = 'A';
      DotStarColor(0xFF0000); // Red
      break;
    case '2':
      _tx = 'B';
      DotStarColor(0x00FF00); // Green
      break;
    case '3':
      _tx = 'C';
      DotStarColor(0x0000FF); // Blue
      break;
    case '4':
      // Start over again
      _tx = '0';
      DotStarColor(0x666666); // Something
      break;
    default:
      _tx = 'Z';
      DotStarColor(0x111111); // dim
      break;
  }

  // Send feedback via USB
  Serial.println(_tx);
}

void VelostatTest()
{
  int sensorPinA = 1;
  int sensorValue = analogRead(sensorPinA);

  Serial.println(sensorValue);

  if (sensorValue > 1000)
    BlinkWithDelay(100);
  else
    delay(100);
}

void loop()
{
  // CommTest();

  VelostatTest();
}