int hapticPin_left = 11;
int hapticPin_right = 10;

//long elapsedtime_left = 0;
//long elapsedtime_right = 0;
//unsigned long startLoop = 0;

unsigned long startPeriod_left = 0;
unsigned long startPeriod_right = 0;
unsigned long startPeriod = 0;
unsigned long endPeriod = 0;

int leftVal = LOW;
int rightVal = LOW; 

int f_left = 2;
int f_right = 2;


void setup() {
  // put your setup code here, to run once:
  pinMode(hapticPin_left, OUTPUT);
  pinMode(hapticPin_right, OUTPUT);

  Serial.begin(9600);

  Serial.println("started");
//  startPeriod_left = millis();
//  startPeriod_right = millis();
}

void loop() {
//  long p = 1000/2;
//  
//  digitalWrite(hapticPin_left, HIGH);
//  
//  startPeriod = millis();
//  delay(p/2);
//  endPeriod = millis();
//  Serial.println(p/2);
//  Serial.println(endPeriod - startPeriod);
//  
//  digitalWrite(hapticPin_left, LOW);
////  startPeriod = millis();
//  delay(p/2);
////  endPeriod = millis();
////  Serial.println(p/2);
////  Serial.println(endPeriod - startPeriod);

  startPeriod_left = millis();
  startPeriod_right = millis();

  while(true){ 
    long period_left = 1000/f_left;
    long period_right = 1000/f_right;

    vibrateBoth(period_left, period_right);

//    if((millis() - startPeriod_left) > (period_left/2)){
//      endPeriod = millis();
//      
//      if(leftVal == LOW){
//        leftVal = HIGH;
//        } 
//      else{
//        leftVal = LOW;
//        }
//      digitalWrite(hapticPin_left, leftVal);
//      startPeriod_left = millis();
//    }
  }
}

void vibrateBoth(long period_left, long period_right){
  if((millis() - startPeriod_left) > (period_left/2)){
    endPeriod = millis();
    
    if(leftVal == LOW){
      leftVal = HIGH;
      } 
    else{
      leftVal = LOW;
      }
    
    digitalWrite(hapticPin_left, leftVal);
    startPeriod_left = millis();
  }

  if((millis() - startPeriod_right) > (period_right/2)){
    
    if(rightVal == LOW){
      rightVal = HIGH;
      } 
    else{
      rightVal = LOW;
      }

    digitalWrite(hapticPin_right, rightVal);
    startPeriod_right = millis();
  }
}
