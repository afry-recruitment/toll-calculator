
public interface Vehicle {

  public String getType();
  default boolean isTollFree() {
    return false;
  }
}
