
int hapticPin_left = 11;
int hapticPin_right = 10;

int inputPin_left = A0;
int inputPin_right = A1;

//-------- VARIABLES ---------------------

char buf[5];

int frequency_left = 0;
int frequency_right = 0;
int prevFreq_left = 0;
int prevFreq_right = 0;

long elapsedtime = 0;
//long duration = 30;
long microduration = 200000;

unsigned long elapsedtime_left = 0;
unsigned long elapsedtime_right = 0;

unsigned long startLoop_left = 0;
unsigned long startLoop_right = 0;

int leftVal = LOW;
int rightVal = LOW; 

void setup() {
  pinMode(hapticPin_left, OUTPUT);
  pinMode(hapticPin_right, OUTPUT);

  pinMode(inputPin_left, INPUT);
  pinMode(inputPin_right, INPUT);

  testVibrate(hapticPin_left, 275);
  testVibrate(hapticPin_right, 275);
}

void loop() {
  frequency_left = analogRead(inputPin_left);
  frequency_right = analogRead(inputPin_right);

  if(frequency_left > 0 && frequency_right > 0){
    longVibrateBoth(frequency_left, frequency_right);
//    Serial.println("exited longvibrateboth");

    if(frequency_left != prevFreq_left){startLoop_left = millis();}
    if(frequency_right != prevFreq_right){startLoop_right = millis();}
  }
  else{
    digitalWrite(hapticPin_left, LOW);
    digitalWrite(hapticPin_right, LOW);
    }

  prevFreq_left = frequency_left;
  prevFreq_right = frequency_right;
}

// ---------- SUB FUNCTIONS ------------------------
void vibrateBoth(int period_left, int period_right){
//  Serial.println("entered vibrateboth");
  
  if((elapsedtime_left % (period_left/2)) == 0){
    if(leftVal == LOW){leftVal = HIGH;} 
    else{leftVal = LOW;}
  }
  
  if((elapsedtime_right % (period_right/2)) == 0){
    if(rightVal == LOW){rightVal = HIGH;} 
    else{rightVal = LOW;}
  }
  digitalWrite(hapticPin_left, leftVal);
  digitalWrite(hapticPin_right, rightVal);
}

void longVibrateBoth(int f_left, int f_right){
  int period_left = (1/f_left)* 100;
  int period_right = (1/f_right)* 100;
  
  vibrateBoth(period_left, period_right);
  
  elapsedtime_left = millis() - startLoop_left;
  elapsedtime_right = millis() - startLoop_right;
}

void testVibrate(int pin, int f){
  int period = (1/f)* 100000;
  while(elapsedtime < microduration){
    digitalWrite(pin, HIGH);
    delayMicroseconds(period/2);
    digitalWrite(pin, LOW);
    delayMicroseconds(period/2);
    elapsedtime += f;
    }
  elapsedtime = 0;
}
