package calculator;

import lombok.extern.slf4j.Slf4j;

import java.io.*;
import java.nio.file.NoSuchFileException;
import java.util.Properties;

@Slf4j
public class PropertiesService
{
    private PropertiesService()
    {
    }

    private static final Properties secretProperties = null;
    private static final Properties calendarRegionsProperties = null;
    private static final Properties settingsProperties = null;
    private static final Properties toolFeeProperties = null;
    public static final String SECRET_PROPERTIES_FILE_NAME = "secrets.properties";
    public static final String CALENDAR_REGIONS_PROPERTIES_NAME = "calendar-regions.properties";
    public static final String SETTINGS_PROPERTIES_FILE_NAME = "app-settings.properties";
    public static final String TOOL_FREE_VEHICLES_PROPERTIES = "toll-free-vehicles.properties";
    public static final String SETTINGS_FOLDER = "settings/";

    private static Properties loadProperties(String fileName)
    {
        Properties properties = null;
        try (InputStream input = new FileInputStream(SETTINGS_FOLDER + fileName))
        {
            properties = new Properties();
            properties.load(input);
        } catch (IOException ex)
        {
            log.error(ex.getMessage());
        }
        return properties;
    }

    private static boolean saveProperties(Properties properties, String fileName)
    {
        File settingsFolder = new File(SETTINGS_FOLDER);
        if (!settingsFolder.exists())
        {
            settingsFolder.mkdirs();
        }
        try (OutputStream os = new FileOutputStream(SETTINGS_FOLDER + fileName))
        {
            properties.store(os, null);
        } catch (IOException ex)
        {
            log.error("Could not store properties to filesystem. " + ex.getMessage());
            return false;
        }
        log.info("Saved property file: " + fileName + " to filesystem. ");
        return true;
    }

    public static String getSecretProperty(String name, String defaultValue)
    {
        return getProperty(name,
                           defaultValue,
                           secretProperties,
                           SECRET_PROPERTIES_FILE_NAME,
                           "Unable to load: " + SECRET_PROPERTIES_FILE_NAME);
    }

    public static String getSettingsProperty(String name, String defaultValue)
    {
        return getProperty(name,
                           defaultValue,
                           settingsProperties,
                           SETTINGS_PROPERTIES_FILE_NAME,
                           "Unable to load: " + SETTINGS_PROPERTIES_FILE_NAME);
    }

    public static boolean setSettingsProperty(String key, String value)
    {
        return setProperty(key,
                           value,
                           settingsProperties,
                           SETTINGS_PROPERTIES_FILE_NAME,
                           "Unable store setting in : " + SETTINGS_PROPERTIES_FILE_NAME);
    }

    public static String getCalendarRegionsProperty(String name, String defaultValue)
    {
        return getProperty(name,
                           defaultValue,
                           calendarRegionsProperties,
                           CALENDAR_REGIONS_PROPERTIES_NAME,
                           "Unable to load: " + CALENDAR_REGIONS_PROPERTIES_NAME);
    }

    public static Properties getProperties(String propertiesName) throws NoSuchFileException
    {
        Properties properties = loadProperties(propertiesName);
        if (properties == null)
        {
            log.error("Unable to load: " + propertiesName);
//            return new Properties();
            throw new NoSuchFileException(propertiesName);
        }
        return properties;
    }

    public static String getToolFreeVehiclesProperty(String name, String defaultValue)
    {
        return getProperty(name,
                           defaultValue,
                           toolFeeProperties,
                           TOOL_FREE_VEHICLES_PROPERTIES,
                           "Unable to load: " + TOOL_FREE_VEHICLES_PROPERTIES);
    }

    public static String getProperty(String name,
                                     String defaultValue,
                                     Properties properties,
                                     String fileName,
                                     String msgOnError)
    {
        if (properties == null)
        {
            properties = loadProperties(fileName);
            if (properties == null) log.error(msgOnError);
        }
        return properties == null ? defaultValue : properties.getProperty(name, defaultValue);
    }


    /*
        todo should save at every change - just app exit or similar
     */
    public static boolean setProperty(String key,
                                      String value,
                                      Properties properties,
                                      String fileName,
                                      String msgOnError)
    {
        if (properties == null)
        {
            properties = loadProperties(fileName);
            if (properties == null)
            {
                log.error(msgOnError);
                return false;
            }
        }
        log.info("Property saved in " + fileName + " with key=" + key + " and value=" + value +
                 ". Resoponse: " + properties.setProperty(key, value));

        return saveProperties(properties, fileName);
    }

}
