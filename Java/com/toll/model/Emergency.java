
public class Emergency implements Vehicle {
  private String number;
  @Override
  public String getType() {
    return "Emergency";
  }

  @Override
  public String getNumber() {
    return number;
  }

  @Override
  public void setNumber(String number) {
    this.number = number;
  }

}
