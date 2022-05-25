#include <SoftwareSerial.h>

SoftwareSerial Serial1(7, 2); // RX, TX

char buf[5];

void setup() {
  // initialize both serial ports:
  Serial.begin(115200);
  Serial1.begin(9600);
}

void loop() {
//  // read from port 1, send to port 0:
//  if (Serial1.available()) {
//    int inByte = Serial1.read();
//    Serial.write(inByte);
//  }
//
//  // read from port 0, send to port 1:
//  if (Serial.available()) {
//    int inByte = Serial.read();
//    Serial1.write(inByte);
//  }

  
//  Serial.readBytesUntil('\n', buf, 5);
//  Serial.println(String(buf[0]));

//  if(Serial1.available() > 0){
//    int inByte = Serial1.read();
//    Serial.println("RECEIVED");
//    Serial.println(inByte);
//  }
//  
//  if(buf[0] != 'T'){
//    Serial.println("ON");
//    Serial1.println(1);
//  }
//  else{
//    Serial.println("OFF");
//    Serial1.println(0);
//  }
//  buf[0] = 'X';
//  delay(2000);


  Serial1.println(1);
  delay(2000);
  Serial1.println(0);
  delay(2000);
  
}
