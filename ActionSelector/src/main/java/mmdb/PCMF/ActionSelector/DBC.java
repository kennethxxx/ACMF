package mmdb.PCMF.ActionSelector;

import java.sql.Connection; 
import java.sql.DriverManager; 
import java.sql.PreparedStatement; 
import java.sql.ResultSet; 
import java.sql.SQLException; 
import java.sql.Statement;
import java.util.HashMap;
import java.util.Map;
import java.util.Properties;

import com.google.gson.Gson;
import com.google.common.reflect.TypeToken;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;
import java.lang.reflect.Type;

import org.json.JSONObject; 

public class DBC {
	
	  private Connection con = null; //Database objects 
	  private Statement stat = null; 
	  private ResultSet rs = null; 
	  private PreparedStatement pst = null;
	  private Properties props;
	  private String driver = null;
	  private String url = null;
	  private String user = null;
	  private String pwd = null;
	  	  
	  public DBC() 
	  { 
		this.loadProperties();
		driver = this.getProperties("driver");
		url = this.getProperties("url");
		user = this.getProperties("user");
		pwd = this.getProperties("password");	
		  
	    try { 
	    	
	    	Class.forName(driver);
	    	con = DriverManager.getConnection(url, user, pwd);
	      
	    	if(con != null && !con.isClosed()) {
	    		System.out.println("database hase connected");
	    	}
	     
	    } 
	    catch(ClassNotFoundException e) 
	    { 
	      System.out.println("DriverClassNotFound :"+e.toString()); 
	    }
	    catch(SQLException e) { 
	      System.out.println("ExceptionAA :"+e.toString()); 
	    } 
	    
	  } 

	  private void loadProperties() {
		  props = new Properties();
		  try {
			  
			  props.load(new FileInputStream("DatabaseConf.properties"));
			  
		  }catch (FileNotFoundException e) { 
			  e.printStackTrace();  
		  }catch (IOException e) {
			  e.printStackTrace();
		  }
		  
	  }
	  
	  private String getProperties(String key) {
		  return props.getProperty(key);
	  }
	  
	  public boolean actionExistedCheck( String action_id ){
		  
		  try
		  {
			  stat = con.createStatement();
			  rs = stat.executeQuery("SELECT action_id FROM action_table WHERE action_id = "+action_id);
			  if( rs.getString("action_id") != null ){  // ture if the action is existed.
				  
				  return true;
				  
			  }else {
				  
				  return false;
				  
			  }
			  
		  }catch(SQLException e){
			  
			  System.out.println("Select from DB Exception:"+e.toString());
			  
		  }finally{
			  
			  Close();
		  }
		  
		return false;
		  
	  }
	  
	  public ActionTask newActionTask( String action_id, String w_task_id ){
		  
		  ActionTask task = new ActionTask();
		  
		  try{	  
		  
			  // create a new action in table
			  String sql = "INSERT INTO action_logger(action_id, w_task_id)";
			  pst = con.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS);
			  pst.setString(0, action_id);
			  pst.setString(1, w_task_id);
			  int last_id = pst.executeUpdate();
			  
			  // got this action info from table and new a task object
			  sql = "SELECT * FROM action_logger WHERE task_id = " + last_id;
			  stat = con.createStatement();
			  rs = stat.executeQuery(sql);
			  while(rs.next()){
				  
				  task.setTaskID(rs.getString("task_id"));
				  task.setActionID(rs.getString("action_id"));
				  task.setWTaskID(rs.getString("w_task_id"));
				  
				  JSONObject input_para_json = new JSONObject(rs.getString("input_para"));
				  Type mapType = new TypeToken<Map<String, Map>>(){}.getType();  
				  HashMap<String, String> input_para = new Gson().fromJson(input_para_json.toString(), mapType);
				  task.setInputPara(input_para);
				  
			  }
		  
		  }catch(Exception e){
			  
			  System.out.println(e.toString());
			  
		  }finally{
			  
			  Close();
			  
		  }
		  
		  return task;
		  	 
	  }
	  
	  public boolean checkIdleWorker( String action_id ) throws ActionNotFoundException {
		  
		  String sql = "SELECT action_owner FROM action_table WHERE action_id = " + action_id;
		  String owner;
		  
		  try {
			  
			  stat = con.createStatement();
			  rs = stat.executeQuery(sql);
			  if( rs.next() ){
				  
				  owner = rs.getString("action_owner");
				  
			  }else {
				  
				  throw new ActionNotFoundException();
				  
			  }
			  
			  sql = "SELECT * FROM worker_table WHERE worker_status = 0 AND worker_type = " + owner;
			  rs = stat.executeQuery(sql);
			  if( rs.next() ) {
				  
				  return true;
				  
			  }else {
				  
				  return false;
				  
			  }
			  
		  }catch(SQLException e) {
			  
			  e.getStackTrace();
			  
		  }
		return false;
		  
	  }
	  
	  public String getWorkerHost(String action_id) throws ActionNotFoundException {
		  
		  String sql = "SELECT action_owner FROM action_table WHERE action_id = " + action_id;
		  String host = null;
		  String owner = null;
		  
		  try {
			  
			  stat = con.createStatement();
			  rs = stat.executeQuery(sql);
			  if( rs.next() ){
				  
				  owner = rs.getString("action_owner");
				  
			  }else {
				  
				  throw new ActionNotFoundException();
				  
			  }
			  
			  sql = "SELECT worker_host FROM worker_table WHERE worker_status = 0 AND worker_type = " + owner;
			  rs = stat.executeQuery(sql);
			  if( rs.next() ) {
				  
				  host = rs.getString("worker_host");
				  
			  }
			  
		  }catch(SQLException e) {
			  
			  e.getStackTrace();
			  
		  }
		  
		  return host;
		  
	  }
	  
	  public String getActionName(String action_id) throws ActionNotFoundException {
		  
		  String sql = "SELECT action_name FROM action_table WHERE action_id = " + action_id;
		  String name = null;
		  
		  try {
			  
			  stat = con.createStatement();
			  rs = stat.executeQuery(sql);
			  if( rs.next() ){
				  
				  name = rs.getString("action_name");
				  
			  }else {
				  
				  throw new ActionNotFoundException();
				  
			  }
			  
		  }catch(SQLException e) {
			  
			  e.getStackTrace();
			  
		  }
		  
		  return name;	  
		  
	  }
	  
	  private void Close() 
	  { 
	    try 
	    { 
	      if(rs!=null) 
	      { 
	        rs.close(); 
	        rs = null; 
	      } 
	      if(stat!=null) 
	      { 
	        stat.close(); 
	        stat = null; 
	      } 
	      if(pst!=null) 
	      { 
	        pst.close(); 
	        pst = null; 
	      } 
	    } 
	    catch(SQLException e) 
	    { 
	      System.out.println("Close Exception :" + e.toString()); 
	    } 
	  } 
	  	  
	  public static void main(String[] args) {
		
		  System.out.println("Test Start.");
		  DBC dbc = new DBC();
		  dbc.Close();
	  
	  }
}
