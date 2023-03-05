// C++ code
//


//Pins
#define LED_PIN  7
#define BUTTON_PIN 6

//Global Variables
unsigned long signal_len, t1, t2;
String code = "";
char morse[4];
int counter = 0;

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
  while(!digitalRead(BUTTON_PIN)){digitalWrite(LED_PIN, LOW);};
  t1 = millis();
  while(digitalRead(BUTTON_PIN)){digitalWrite(LED_PIN, HIGH);};
  t2 = millis();
  
  signal_len = t2 - t1;
  
  
  // Check seginal_len for eitehr dot or dash
  //store dot or dash into char array 
  //ummmmmmmmmmmmmmmmmmm
  //still trying to figure out what do after that but it's a start
  
  if(signal_len <= 100 && counter != 3)
  {
      morse[counter] = '.';
      counter++;
    
  } 
  
  
  if(signal_len >= 300 && counter != 3)
  {
  
      morse[counter] = '-';
      counter++;
  }
  
  if(counter == 3)
  {
   	for(int i = 0; i < 4; i++)
    {
      Serial.print(morse[i]);
    }
    counter = 0;
    Serial.println("");
    Serial.println("reseting");
  }
  
  delay(500);
  
  
}