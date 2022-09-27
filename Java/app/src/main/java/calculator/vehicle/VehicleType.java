package calculator.vehicle;

import javax.annotation.Nonnull;
import java.util.Objects;

public class VehicleType
{
    private final String type;
    private final Boolean tollFree;

    public VehicleType(@Nonnull String type, @Nonnull Boolean tollFree)
    {
        this.type = type;
        this.tollFree = tollFree;
    }

    public String getType()
    {
        return type;
    }

    public Boolean isTollFree()
    {
        return tollFree;
    }

    /**
     * Checks equality between this and other object. Case of the String type is not considered.
     * @param object object to compare to
     * @return equality of this and object
     */
    @Override
    public boolean equals(Object object)
    {
        if (this == object) return true;
        if (!(object instanceof VehicleType)) return false;
        VehicleType that = (VehicleType) object;
        return getType().equalsIgnoreCase(that.getType()) && isTollFree().equals(that.isTollFree());
    }

    @Override
    public int hashCode()
    {
        return Objects.hash(getType().toLowerCase(), isTollFree());
    }
}

