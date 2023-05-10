const int buttonPins[] = {2, 3, 4, 5, 6, 7, 8, 9, 10, 11}; // set the array of Arduino pins used by switches (buttons)
// switches 1-10 are wired to pins 2-11

const int numButtons = sizeof(buttonPins)/sizeof(buttonPins[0]); // calculate the number of buttons

int buttonStates[numButtons];

int lastSwitchStates[numButtons] = {1, 1, 1, 1, 1, 1, 1, 1, 1, 1};  // sets starting lastSwitchStates to unpressed

void setup() {
  Serial.begin(9600);  // initialize serial communication
  for (int i = 0; i < numButtons; i++) {
    pinMode(buttonPins[i], INPUT_PULLUP); // set pinMode to input_pullup for pins in the buttonPins list
    // this ties each input to +5V (through resistors integral to the chip) as an anchor.  Physical switch closes input pin to ground.
    // if switch is open, pin will read HIGH, or 5V.  If switch is closed or activated, pin will read LOW.
  }
  pinMode(13, OUTPUT); // set pin 13 to output (pin 13 not wired, but program uses on-board LED tied to pin13)
}

void loop() {
  for (int i = 0; i < 10; i++) {
    buttonStates[i] = digitalRead(buttonPins[i]);  // read the button state
      if (buttonStates[i] != lastSwitchStates[i]) {  // look to see if state has changed
      if (buttonStates[i] == LOW) {
            Serial.println(i); // if any switch has been activated, print to serial the coresponding # (SW 1-10 prints 0-9)
      }
      } 
    lastSwitchStates[i] = buttonStates[i];  // change the last state to the current so it only prints once
    delay(10);
    }
  if (buttonStates[0] == LOW || buttonStates[1] == LOW || buttonStates[2] == LOW || buttonStates[3] == LOW || buttonStates[4] == LOW || buttonStates[5] == LOW || buttonStates[6] == LOW || buttonStates[7] == LOW || buttonStates[8] == LOW || buttonStates[9] == LOW) {
      digitalWrite(13, HIGH); // if any switch is on (input pin reading "LOW"), turn on pin 13 output, activating the on-board LED
      // this LED assists troubleshooting- if any switch is closed, the LED should be lit up.
    }
  else {
    digitalWrite(13, LOW);
    }
}

// The core of this program written by Travis Winterton as part of the UAF Computer Science Senior Capstone class team developing the Virtual Morse 2023
// upgrade during the spring of 2023.  Comments and modifications to include pin 13 LED and monitoring state change of switches was by Ben
// using public examples.
