package mmdb.PCMF.EncapsulationManager.PackageDeployer;

import java.sql.Connection; 
import java.sql.DriverManager; 
import java.sql.PreparedStatement; 
import java.sql.ResultSet; 
import java.sql.SQLException; 
import java.sql.Statement;
import java.util.ArrayList;
import java.util.HashMap;

import java.util.Properties;

import java.io.FileInputStream;
import java.io.FileNotFoundException;
import java.io.IOException;

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
			  
			  props.load(new FileInputStream("/home/hduser/pcmf/DatabaseConf.properties"));
			  
		  }catch (FileNotFoundException e) { 
			  e.printStackTrace();  
		  }catch (IOException e) {
			  e.printStackTrace();
		  }
		  
	  }
	  
	  private String getProperties(String key) {
		  return props.getProperty(key);
	  }
	  
	  
	  public void newAction(HashMap<String, String> action_info){
		  
		  try{	  
		  
			  // create a new action in table
			  String sql = "INSERT INTO action_table(action_owner, action_name, action_input, action_output, action_method, contetn_type, prefix) "
			  		+ "VALUES (?,?,?,?,?,?,?)";
			  pst = con.prepareStatement(sql, Statement.RETURN_GENERATED_KEYS);
			  pst.setString(1, action_info.get("action_owner"));
			  pst.setString(2, action_info.get("action_name"));
			  pst.setString(3, action_info.get("action_input"));
			  pst.setString(4, action_info.get("action_output"));
			  pst.setString(5, action_info.get("action_method"));
			  pst.setString(6, action_info.get("content_type"));
			  pst.setString(7, action_info.get("prefix"));
			  
			  System.out.println("SQL: " + pst.toString());
			  pst.executeUpdate();
		  
		  }catch(Exception e){
			  
			  e.printStackTrace();
			 
		  }finally{
			  
			  Close();
			  
		  }
		  	 
	  }
	  
	  public ArrayList<String> getWorkerHost(String owner) {
		  
		  String id = null;
		  ArrayList<String> hosts = new ArrayList<String>();
		  
		  try {
			  
			  String sql = "SELECT worker_host FROM worker_table WHERE worker_type = '" + owner + "'";
			  rs = stat.executeQuery(sql);
			  while( rs.next() ) {
				  hosts.add(rs.getString("worker_host"));
			  }
			  
		  }catch(SQLException e) {
			  
			  e.getStackTrace();
			  
		  }
		  
		  return hosts;
		  
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
			
	  
	  }
}
