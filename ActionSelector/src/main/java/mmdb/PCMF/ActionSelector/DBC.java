package mmdb.PCMF.ActionSelector;

import java.sql.Connection; 
import java.sql.DriverManager; 
import java.sql.PreparedStatement; 
import java.sql.ResultSet; 
import java.sql.SQLException; 
import java.sql.Statement;
import java.util.HashMap;

import java.util.Properties;
import org.apache.log4j.PropertyConfigurator;
import org.apache.log4j.Logger;

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
			  
			  props.load(new FileInputStream("../../DatabaseConf.properties"));
			  
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
	  
	  public ActionTask newActionTask( String action_id, String w_task_id, String task_input ){
		  
		  ActionTask task = new ActionTask();
		  
		  try{	  
		  
			  // create a new action in table
			  String sql = "INSERT INTO action_logger(action_id, process_id, task_input) VALUES (?,?,?)";
			  pst = con.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS);
			  pst.setString(1, action_id);
			  pst.setString(2, w_task_id);
			  pst.setString(3, task_input);
			  System.out.println("SQL: " + pst.toString());
			  pst.executeUpdate();
			  Long last_id = null;
			  ResultSet genkey = pst.getGeneratedKeys();
			  if (genkey.next()) {
				  last_id = genkey.getLong(1);
			  }else {
				  throw new SQLException("Creating user failed, no ID obtained.");
			  }
			  
			  // got this action info from table and new a task object
			  sql = "SELECT * FROM action_logger WHERE task_id = " + last_id;
			  stat = con.createStatement();
			  rs = stat.executeQuery(sql);
			  while(rs.next()){
				  
				  task.setTaskID(rs.getString("task_id"));
				  task.setActionID(rs.getString("action_id"));
				  task.setWTaskID(rs.getString("process_id"));

				  JSONObject input_para_json = new JSONObject(rs.getString("task_input"));
				  Type mapType = new TypeToken<HashMap<String, String>>(){}.getType();  
				  HashMap<String, String> input_para = new Gson().fromJson(input_para_json.toString(), mapType);
				  task.setInputPara(input_para);
		  
			  }
		  
		  }catch(Exception e){
			  
			  e.printStackTrace();
			 
		  }finally{
			  
			  Close();
			  
		  }
		  
		  return task;
		  	 
	  }
	  
	  public boolean checkIdleWorker( String action_id ) throws ActionNotFoundException {
		  
		  String sql = "SELECT action_owner FROM action_table WHERE action_id = " + action_id;
		  System.out.println("Action Check SQL: " + sql);
		  String owner;
		  
		  try {
			  
			  stat = con.createStatement();
			  rs = stat.executeQuery(sql);
			  if( rs.next() ){
				  
				  owner = rs.getString("action_owner");
				  
			  }else {
				  
				  throw new ActionNotFoundException();
				  
			  }
			  
			  sql = "SELECT * FROM worker_table WHERE worker_status = 0 AND worker_type = '" + owner + "'";
			  System.out.println("worker Check SQL: " + sql);
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
			  
			  sql = "SELECT worker_host FROM worker_table WHERE worker_status = 0 AND worker_type = '" + owner + "'";
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

	  public String getActionMethod(String action_id) throws ActionNotFoundException {
		  
		  String sql = "SELECT action_method FROM action_table WHERE action_id = " + action_id;
		  String method = null;
		  
		  try {
			  
			  stat = con.createStatement();
			  rs = stat.executeQuery(sql);
			  if( rs.next() ){
				  
				  method = rs.getString("action_method");
				  
			  }else {
				  
				  throw new ActionNotFoundException();
				  
			  }
			  
		  }catch(SQLException e) {
			  
			  e.getStackTrace();
			  
		  }
		  
		  return method;	  
		  
	  }	  	  
	  
	  public void logExecutingResult(String task_id, HashMap<String, String> task_result) {
		  
		  String sql = "UPDATE action_logger SET `task_result` = ?," +
				  								"`status` = ? " +
				  							"WHERE `task_id` = ?";
		  try {
			  pst = con.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS);
			  pst.setString(1, new JSONObject(task_result).toString());
			  pst.setInt(2, 2);
			  pst.setString(3, task_id);
			  System.out.println(pst.toString());
			  pst.executeUpdate();
			  System.out.println("Record is updated to action_logger table");

		  }catch (SQLException e) {
			  
			  e.printStackTrace();
			  
		  }

		  
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
			
		  PropertyConfigurator.configure("log4j.properties");
		  Logger logger = Logger.getLogger(DBC.class);
		  
		  logger.info("Test Start.");
		  DBC dbc = new DBC();
		  HashMap<String, String> input_para = new HashMap<String, String>();
		  input_para.put("test","1");
		  ActionTask task = dbc.newActionTask("0", "1", null);	
		  dbc.Close();
		  Dispatcher patcher = new Dispatcher(task);
		  patcher.run();
		  logger.info("Test End");
		
	  
	  }
}
