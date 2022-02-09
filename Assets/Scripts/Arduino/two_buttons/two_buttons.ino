int fine = round(1000000/300);
int mid = round(1000000/250);
int coarse = round(1000000/200);

int leftPWM = 10;
int rightPWM = 9;

int leftSensor = A0;
int rightSensor = A1;

//long elapsedtime = 0;
//long duration = 8000000;

bool leftPressed;
bool rightPressed;

int sensedLast;

int  ctone = 3830;  // 3830 261 Hz
int  dtone = 3400;  // 294 Hz
int  etone = 3038;  // 329 Hz

long elapsedtime = 0;
long duration = 8000000;  // play each tone for about a half second (microseconds)

void setup() {
  Serial.begin(9600);
  Serial.println("start");
//  pinMode(leftPWM, OUTPUT);
//  pinMode(rightPWM, OUTPUT);

//  pinMode(leftSensor, INPUT);
//  pinMode(rightSensor, INPUT);
}

void loop() { 
  int currentFreq = mid;
  int currentPin = selectPin();
//  int currentPin = 9;
//  demo();
//  vibrate(currentPin, currentFreq);
  if(currentPin > 0){
    vibrate(currentPin, currentFreq);
  }
//  delay(2);
//  Serial.println(currentPin);
//  int freq = selectFreq();
//  if (freq > 0){
//    vibrate(
//  }

}

int selectPin(){
  int threshold = 10;
  int curPin = 0;

  int lSense = analogRead(leftSensor);
  int rSense = analogRead(rightSensor);

  Serial.print(lSense);
  Serial.print(" ");
  Serial.println(rSense);

  if (lSense > threshold){
    curPin = leftPWM;
  }

  if (rSense > threshold){
    curPin = rightPWM;
  }
  
  return curPin;
}

int selectFreq(){
  int freq;
  
  if(Serial.available()){
    char c = Serial.read();
    
    if (c){
      if(c == "F"){
        freq = fine;
      }

      else if(c == "M"){
        freq = mid;
      }

      else if(c == "C"){
        freq = coarse;
      }
    }
  }
  
  return freq;
}

void vibrate(int pin, int f){
  pinMode(pin, OUTPUT);
  
  digitalWrite(pin, HIGH);
  delayMicroseconds(f/2);
  digitalWrite(pin, LOW);
  delayMicroseconds(f/2);  

//  pinMode(pin, INPUT);
}

void demo(){
  while(elapsedtime < duration) {
    digitalWrite(rightPWM, HIGH);
    delayMicroseconds(ctone / 2);
    digitalWrite(rightPWM, LOW);
    delayMicroseconds(ctone / 2);
    elapsedtime += ctone;
  }
}
