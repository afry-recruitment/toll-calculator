
public class Foreign implements Vehicle {
  private String number;
  @Override
  public String getType() {
    return "Foreign";
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
