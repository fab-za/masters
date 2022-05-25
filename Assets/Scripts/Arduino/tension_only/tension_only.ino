#include <Servo.h>
#include <SoftwareSerial.h>

Servo tensionMotor_left;
Servo tensionMotor_right;

int motorPin_left = 6;
int motorPin_right = 5;

int hapticOutputPin_left = A0;
int hapticOutputPin_right = A1;

SoftwareSerial Serial2(7, 2); // RX, TX

//-------- VARIABLES ---------------------

char buf[5];

int slack = 90;
int tense = 20;

int train1 = 20;
int train2 = 80;
int train3 = 200;

int test1 = 280;
int test2 = 260;
int test3 = 290;
//int test4 = 270;
//int test5 = 270;

int frequency_left = 0;
int frequency_right = 0;

char tensionMode_left = 'S';
char tensionMode_right = 'S';
int vibrationMode_left = 'A';
int vibrationMode_right = 'A';

int count = 0;

void setup() {  
  pinMode(hapticOutputPin_left, OUTPUT);
  pinMode(hapticOutputPin_right, OUTPUT);
  
  tensionMotor_left.attach(motorPin_left);
  tensionMotor_right.attach(motorPin_right);

  Serial.begin(115200);
  Serial.setTimeout(200);

  Serial2.begin(9600);
  Serial2.setTimeout(200);

  while(Serial.available()>0){Serial.read();}

  tensionMotor_left.write(110);
  tensionMotor_right.write(70);

  delay(500);
  
  tensionMotor_left.write(slack);
  tensionMotor_right.write(slack);

  Serial.print("start");
//  Serial.println(millis());
}

void loop() {
//  buf[0] = 'X';
//  Serial.println(Serial.available());
//  Serial.println(millis());
//  long check = millis();
  Serial.readBytesUntil('\n', buf, 5);
//  long endcheck = millis();
  Serial.println(String(buf[0])+String(buf[1])+String(buf[2])+String(buf[3]));
  
  tensionMode_left = buf[0];
  tensionMode_right = buf[1];
  vibrationMode_left = buf[2];
  vibrationMode_right = buf[3];

  
  moveTensionMotor(tensionMotor_left, tensionMode_left, 1);
  moveTensionMotor(tensionMotor_right, tensionMode_right, -1);
  

  frequency_left = vibrationModeToFrequency(vibrationMode_left);
  frequency_right = vibrationModeToFrequency(vibrationMode_right);

  if(frequency_left > -1){
    analogWrite(hapticOutputPin_left, frequency_left);
    Serial.println(frequency_left);
    Serial2.println(frequency_left);

  }
  if(frequency_right > -1){
    analogWrite(hapticOutputPin_right, frequency_right);
//    Serial2.println(frequency_right);
  }

//  Serial.println(analogRead(A5));
//  Serial.println(endcheck - check);

//  Serial.println(Serial2.read());
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

int vibrationModeToFrequency(char c) {
  int frequency;

  if(c == 'A'){frequency = 0;}
  
  else if(c == 'N'){frequency = train1;}  
  else if(c == 'Q'){frequency = train2;}
  else if(c == 'S'){frequency = train3;}
  
  else if(c == 'E'){frequency = test1;}
  else if(c == 'F'){frequency = test2;}
  else if(c == 'G'){frequency = test3;}
//  else if(c == 'H'){frequency = test4;}
//  else if(c == 'I'){frequency = test5;}

  return frequency;
}
