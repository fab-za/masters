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
int train2 = 300;
int train3 = 400;

int test1 = 280;
int test2 = 260;
int test3 = 290;
//int test4 = 270;
//int test5 = 270;

char tensionMode_left = 'S';
char tensionMode_right = 'S';
int vibrationMode_left = 'A';
int vibrationMode_right = 'A';

void setup() {  
  pinMode(hapticOutputPin_left, OUTPUT);
  pinMode(hapticOutputPin_right, OUTPUT);
  
  tensionMotor_left.attach(motorPin_left);
  tensionMotor_right.attach(motorPin_right);

  Serial.begin(9600);
//  Serial1.begin(9600);
  Serial2.begin(9600);
  
  Serial.setTimeout(200);
//  Serial1.setTimeout(200);
  Serial2.setTimeout(200);

  tensionMotor_left.write(110);
  tensionMotor_right.write(70);

  delay(500);
  
  tensionMotor_left.write(slack);
  tensionMotor_right.write(slack);
}

void loop() {
  String message = Serial.readStringUntil('\n');
//  Serial.println("message received: " + message);

  if(message != ""){
    message.toCharArray(buf, 5);
  }

//  delay(10);

  tensionMode_left = buf[0];
  tensionMode_right = buf[1];
  vibrationMode_left = buf[2];
  vibrationMode_right = buf[3];

//  Serial.println("left tension received: " + tensionMode_left);

  moveTensionMotor(tensionMotor_left, tensionMode_left, 1);
  moveTensionMotor(tensionMotor_right, tensionMode_right, -1);

  analogWrite(hapticOutputPin_left, vibrationModeToFrequency(vibrationMode_left));
  analogWrite(hapticOutputPin_right, vibrationModeToFrequency(vibrationMode_right));
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
