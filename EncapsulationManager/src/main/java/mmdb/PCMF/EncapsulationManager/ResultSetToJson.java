package mmdb.PCMF.EncapsulationManager;

import java.sql.ResultSet;
import java.sql.ResultSetMetaData;
import java.sql.SQLException;
import com.google.gson.*;
 
public class ResultSetToJson {
    public static final JsonArray ResultSetToJsonArray(ResultSet rs) {
        JsonObject element = null;
        JsonArray ja = new JsonArray();
        ResultSetMetaData rsmd = null;
        String columnName, columnValue = null;
        try {
            rsmd = rs.getMetaData();
            while (rs.next()) {
                element = new JsonObject();
                for (int i = 0; i < rsmd.getColumnCount(); i++) {
                    columnName = rsmd.getColumnName(i + 1);
                    columnValue = rs.getString(columnName);
                    element.addProperty(columnName, columnValue);
                }
                ja.add(element);
            }
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return ja;
    }
 
    public static final JsonObject ResultSetToJsonObject(ResultSet rs) {
        JsonObject element = null;
        JsonArray ja = new JsonArray();
        JsonObject jo = new JsonObject();
        ResultSetMetaData rsmd = null;
        String columnName, columnValue = null;
        try {
            rsmd = rs.getMetaData();
            while (rs.next()) {
                element = new JsonObject();
                for (int i = 0; i < rsmd.getColumnCount(); i++) {
                    columnName = rsmd.getColumnName(i + 1);
                    columnValue = rs.getString(columnName);
                    element.addProperty(columnName, columnValue);
                }
                ja.add(element);
            }
            jo.add("result", ja);
        } catch (SQLException e) {
            e.printStackTrace();
        }
        return jo;
    }
 
    public static final String ResultSetToJsonString(ResultSet rs) {
        return ResultSetToJsonObject(rs).toString();
    }
}