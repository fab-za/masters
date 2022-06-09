#include <Servo.h>

Servo tensionMotor_left;
Servo tensionMotor_right;

int motorPin_left = 10;
int motorPin_right = 11;

int outputPins[2][5] = {{5,4,3,2,A5}, {A4,A3,A2,A1,A0}};

//-------- VARIABLES ---------------------

char buf[5];

int slack_right = 108;
int tense_right = 128;

int slack_left = 89;
int tense_left = 78;

long frequencyMode_left = 0;
long frequencyMode_right = 0;

char tensionMode_left = 'Z';
char tensionMode_right = 'Z';
int vibrationMode_left = 'Z';
int vibrationMode_right = 'Z';

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

  tensionMotor_left.write(tense_left);
  tensionMotor_right.write(tense_right);

//  delay(1000);
//
//  tensionMotor_left.write(slack_left);
//  tensionMotor_right.write(slack_right);


//  Serial.print("start");
//  Serial.println(millis());
}

void loop() {
//  buf[0] = 'X';
//  Serial.println(Serial.available());
//  Serial.println(millis());

//  check = micros();

  
  if(Serial.available() > 0){
    Serial.readBytesUntil('\n', buf, 5);
  }



  
//  Serial.println(String(buf[0])+String(buf[1])+String(buf[2])+String(buf[3]));

  if(buf[2] != vibrationMode_left){
    vibrationMode_left = buf[2];
    frequencyMode_left = alphabet.indexOf(vibrationMode_left);
    intToBinary(frequencyMode_left, 5, 0);
  }
  if(buf[3] != vibrationMode_right){
    vibrationMode_right = buf[3];
    frequencyMode_right = alphabet.indexOf(vibrationMode_right);
    intToBinary(frequencyMode_right, 5, 1);    
  }
  if(buf[0] != tensionMode_left){
    tensionMode_left = buf[0];
    moveTensionMotorLeft(tensionMode_left);
  }
  if(buf[1] != tensionMode_right){
    tensionMode_right = buf[1];    
    moveTensionMotorRight(tensionMode_right);
  }

//  endcheck = micros();
//  Serial.println(check);
//  Serial.println(endcheck);
//  Serial.println(endcheck - check);
}

void moveTensionMotorRight(char mode) {
  if (mode == 'S') {
    tensionMotor_right.write(slack_right);
  }
  else if (mode == 'T') {
    tensionMotor_right.write(tense_right);
  }
}

void moveTensionMotorLeft(char mode) {
  if (mode == 'S') {
    tensionMotor_left.write(slack_left);
  }
  else if (mode == 'T') {
    tensionMotor_left.write(tense_left);
  }
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

// ------------- OLD
//void moveTensionMotorLeft(char mode) {
//  int pos;
//  if (mode == 'S') {
//    for (pos = tense_left; pos <= slack_left; pos += 1) {
//      tensionMotor_left.write(pos);              
//      intToBinary(frequencyMode_left, 5, 0);  
//      intToBinary(frequencyMode_right, 5, 1);                    
//    }
//  }
//  else if (mode == 'T') {
//    for (pos = slack_left; pos >= tense_left; pos -= 1) {
//      tensionMotor_left.write(pos);              
//      intToBinary(frequencyMode_left, 5, 0);  
//      intToBinary(frequencyMode_right, 5, 1);                    
//    }
//  }
//}
//
//void moveTensionMotorRight(char mode) {
//  int pos;
//  if (mode == 'S') {
//    for (pos = tense_right; pos >= slack_right; pos -= 1) {
//      tensionMotor_right.write(pos);              
//      intToBinary(frequencyMode_left, 5, 0);  
//      intToBinary(frequencyMode_right, 5, 1);                    
//    }
//  }
//  else if (mode == 'T') {
//    for (pos = slack_right; pos <= tense_right; pos += 1) {
//      tensionMotor_right.write(pos);              
//      intToBinary(frequencyMode_left, 5, 0);  
//      intToBinary(frequencyMode_right, 5, 1);                    
//    }
//  }
//}
