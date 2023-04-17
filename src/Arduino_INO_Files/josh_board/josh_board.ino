// C++ code


#define buttonPin2 2
#define buttonPin3 3
#define buttonPin4 4
#define buttonPin5 5
#define buttonPin6 6
#define buttonPin7 7
#define buttonPin8 8
#define buttonPin9 9
#define buttonPin10 10
#define buttonPin11 11




void setup()
{
  Serial.begin(9600);
  pinMode(buttonPin2, INPUT_PULLUP);
  pinMode(buttonPin3, INPUT_PULLUP);
  pinMode(buttonPin4, INPUT_PULLUP);
  pinMode(buttonPin5, INPUT_PULLUP);
  pinMode(buttonPin6, INPUT_PULLUP);
  pinMode(buttonPin7, INPUT_PULLUP);
  pinMode(buttonPin8, INPUT_PULLUP);
  pinMode(buttonPin9, INPUT_PULLUP);
  pinMode(buttonPin10,INPUT_PULLUP);
  pinMode(buttonPin11,INPUT_PULLUP);
  
}

void loop()
{
  for(int i = 2; i <= 11; i++)
    checkButton(i);
  delay(100);
}

void checkButton(int pinNum)
{
  int pushed = digitalRead(pinNum);
  
  if(pushed == LOW)
  {
    Serial.print(pinNum);
    Serial.println();
    
  }
 
}
