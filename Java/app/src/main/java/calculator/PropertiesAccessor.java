package calculator;

import lombok.extern.slf4j.Slf4j;

import java.io.IOException;
import java.io.InputStream;
import java.util.Properties;

@Slf4j
public class PropertiesAccessor
{
    private static Properties secretProperties = null;
    private static final String SECRET_PROPERTY_FILE_NAME = "secrets.properties";

    private static boolean loadSecretProperty()
    {
        try (InputStream input = PropertiesAccessor.class.getClassLoader()
                                                         .getResourceAsStream(SECRET_PROPERTY_FILE_NAME))
        {
            if (input != null)
            {
                Properties prop = new Properties();
                prop.load(input);
                secretProperties = prop;
                return true;
            }
        } catch (IOException ex)
        {
            log.error(ex.getMessage());

        }
        return false;
    }

    public static Object getSecretProperty(String name, String defaultProperty)
    {
        if (secretProperties == null)
            if (loadSecretProperty()) return secretProperties.getProperty(name, defaultProperty);
        log.error("Unable get property: " + name);
        return defaultProperty;
    }
}
