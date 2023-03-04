// C++ code
//


//Pins
#define LED_PIN  7
#define BUTTON_PIN 6

//Global Variables
unsigned long signal_len, t1, t2;
String code = "";

void setup()
{
  pinMode(LED_PIN, OUTPUT);
  pinMode(BUTTON_PIN, INPUT_PULLUP);
  Serial.begin(9600);
  Serial.println("Press button to start inputing morse code");
  
  while(!digitalRead(BUTTON_PIN));
}



void loop()
{
  
  //Start timer when button pushed (t1)
  //keep track of time untill BUTTON_PIN = low (t2)
  //Find total signal length to determin dot or dash
  while(!digitalRead(BUTTON_PIN)){};
  t1 = millis();
  while(digitalRead(BUTTON_PIN)){};
  t2 = millis();
  
  signal_len = t2 - t1;
  
  if(signal_len <= 100)
  {
   Serial.print("."); 
  }
  
  if(signal_len >= 300)
  {
    Serial.print("-");
  }
  

  	
}