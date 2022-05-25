#include <Servo.h>

Servo tensionMotor_left;
Servo tensionMotor_right;

int motorPin_left = 11;
int motorPin_right = 10;

int outputPins[2][5] = {{5,4,3,2,A5}, {A4,A3,A2,A1,A0}};

//-------- VARIABLES ---------------------

char buf[5];

int slack = 100;
int tense = 20;

long frequencyMode_left = 0;
long frequencyMode_right = 0;

char tensionMode_left = 'S';
char tensionMode_right = 'S';
int vibrationMode_left = 'A';
int vibrationMode_right = 'A';

String alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

long check;
long endcheck;

void setup() {  
  tensionMotor_left.attach(motorPin_left);
  tensionMotor_right.attach(motorPin_right);

  for(int i=0; i<5; i++)  {pinMode(outputPins[0][i], OUTPUT);}
  for(int i=0; i<5; i++)  {pinMode(outputPins[1][i], OUTPUT);}

  Serial.begin(9600);
  Serial.setTimeout(200);

  tensionMotor_left.write(110);
  tensionMotor_right.write(70);

  delay(500);
  
  tensionMotor_left.write(slack);
  tensionMotor_right.write(slack);

//  Serial.print("start");
//  Serial.println(millis());
}

void loop() {
//  buf[0] = 'X';
//  Serial.println(Serial.available());
//  Serial.println(millis());
    
  Serial.readBytesUntil('\n', buf, 5);
  
//  Serial.println(String(buf[0])+String(buf[1])+String(buf[2])+String(buf[3]));
  
  tensionMode_left = buf[0];
  tensionMode_right = buf[1];
  vibrationMode_left = buf[2];
  vibrationMode_right = buf[3];

  moveTensionMotor(tensionMotor_left, tensionMode_left, 1);
  moveTensionMotor(tensionMotor_right, tensionMode_right, -1);

//  check = millis();
  frequencyMode_left = alphabet.indexOf(vibrationMode_left);
  frequencyMode_right = alphabet.indexOf(vibrationMode_right);

  intToBinary(frequencyMode_left, 5, 0);
  intToBinary(frequencyMode_right, 5, 1);
//  endcheck = millis();

//  Serial.println(endcheck - check);
}

void moveTensionMotor(Servo motor, char mode, int dir) {
  if (mode == 'S') {
    motor.write(slack);
  }
  else if (mode == 'T') {
    motor.write(slack + (tense * dir));
  }
//  delay(100);
}

void intToBinary(int f, int s, int side){
  bool buf[s];
  byte fb = f;
  
  for(int i=(s-1); i>=0; i--)
  {
    bool m = bitRead(f, i);
    buf[i] = m;
  }

//  printbuf(buf, s);
  toPin(buf, s, side);
}

void printbuf(bool buf[], int s){
  for(int i=(s-1); i>=0; i--){
    Serial.print(buf[i]);
  }
  Serial.println(" ");
}

void toPin(bool buf[], int s, int side){
  for(int i=(s-1); i>=0; i--){
    digitalWrite(outputPins[side][i], buf[i]);
  }
}
