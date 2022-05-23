
int hapticPin_left = 11;
int hapticPin_right = 10;

int inputPin_left = A0;
int inputPin_right = A1;

//-------- VARIABLES ---------------------

char buf[5];

long elapsedtime = 0;
long elapsedtime_left = 0;
long elapsedtime_right = 0;

long duration = 30;
long microduration = 200000;

int frequency_left = 0;
int frequency_right = 0;
int prevFreq_left = 0;
int prevFreq_right = 0;

unsigned long startLoop = 0;
unsigned long startVibration = 0;
unsigned long leftTime = 0;
unsigned long rightTime = 0;
int leftVal = LOW;
int rightVal = LOW; 

void setup() {
  pinMode(hapticPin_left, OUTPUT);
  pinMode(hapticPin_right, OUTPUT);

  pinMode(inputPin_left, INPUT);
  pinMode(inputPin_right, INPUT);

  longVibrate(hapticPin_left, 275);
  longVibrate(hapticPin_right, 275);
}

void loop() {
  frequency_left = analogRead(inputPin_left);
  frequency_right = analogRead(inputPin_right);

  if(frequency_left > 0 && frequency_right > 0){
    longVibrateBoth(frequency_left, frequency_right);
//    Serial.println("exited longvibrateboth");

    if(frequency_left != prevFreq_left){elapsedtime_left = 0;}
    if(frequency_right != prevFreq_right){elapsedtime_right = 0;}
  }
  else{
    digitalWrite(hapticPin_left, LOW);
    digitalWrite(hapticPin_right, LOW);
    }

  prevFreq_left = frequency_left;
  prevFreq_right = frequency_right;
}

// ---------- SUB FUNCTIONS ------------------------
void vibrateBoth(int f_left, int f_right){
//  Serial.println("entered vibrateboth");
  
  if((elapsedtime_left % (f_left/2)) == 0){
    if(leftVal == LOW){leftVal = HIGH;} 
    else{leftVal = LOW;}
  }
  
  if((elapsedtime_right % (f_right/2)) == 0){
    if(rightVal == LOW){rightVal = HIGH;} 
    else{rightVal = LOW;}
  }
  digitalWrite(hapticPin_left, leftVal);
  digitalWrite(hapticPin_right, rightVal);
}

void longVibrateBoth(int f_left, int f_right){  
  vibrateBoth(f_left, f_right);
  elapsedtime_left = millis() - startLoop;
  elapsedtime_right = millis() - startLoop;
}

void longVibrate(int pin, int f){
  while(elapsedtime < microduration){
    vibrate(pin, f);
    elapsedtime += f;
    }
  elapsedtime = 0;
}

void vibrate(int pin, int f){
  digitalWrite(pin, HIGH);
  delayMicroseconds(f/2);
  digitalWrite(pin, LOW);
  delayMicroseconds(f/2);
}
