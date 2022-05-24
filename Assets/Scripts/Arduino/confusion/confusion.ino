int hapticPin_left = 11;
int hapticPin_right = 10;

long elapsedtime_left = 0;
long elapsedtime_right = 0;
unsigned long startLoop = 0;

int leftVal = LOW;
int rightVal = LOW; 

void setup() {
  // put your setup code here, to run once:
  pinMode(hapticPin_left, OUTPUT);
  pinMode(hapticPin_right, OUTPUT);

  Serial.begin(9600);
  Serial.setTimeout(1000);

  Serial.println("started");
}

void loop() {
//  Serial.println("loop");
  delay(10);
  
  int p = 100000/200;
  digitalWrite(hapticPin_left, HIGH);
  delayMicroseconds(p/2);
  digitalWrite(hapticPin_left, LOW);
  delayMicroseconds(p/2);
  
//  longVibrateBoth(20, 20);
}

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
  f_left = (1/f_left)* 1000;
  f_right = (1/f_right)* 1000;
  vibrateBoth(f_left, f_right);
  elapsedtime_left = millis() - startLoop;
  elapsedtime_right = millis() - startLoop;

  Serial.println("left period: " + f_left);
  Serial.println("elapsed time: " + elapsedtime_left);
}

void otherLoop() {
  int p = 100000/200;
  digitalWrite(hapticPin_left, HIGH);

  startPeriod = millis();
  delayMicroseconds(p/2);
  Serial.println(millis()-startPeriod);
  
  digitalWrite(hapticPin_left, LOW);
  
  startPeriod = millis();
  delayMicroseconds(p/2);
  Serial.println(millis()-startPeriod);
  
  int period_left = 1000/f_left;
  int period_right = 1000/f_right;
  vibrateBoth(period_left, period_right);

  while(true){
    startPeriod_left = millis();
    startPeriod_right = millis();

    int period_left = 1000/f_left;
    int period_right = 1000/f_right;
    vibrateBoth(period_left, period_right);
  }
}
