package mmdb.WTF.ActionSelector;

import java.sql.Connection; 
import java.sql.DriverManager; 
import java.sql.PreparedStatement; 
import java.sql.ResultSet; 
import java.sql.SQLException; 
import java.sql.Statement;
import java.util.HashMap;
import java.util.Map;

import com.google.gson.Gson;
import com.google.common.reflect.TypeToken;
import java.lang.reflect.Type;

import org.json.JSONObject; 

public class DBC {
	
	  private Connection con = null; //Database objects 
	  //連接object 
	  private Statement stat = null; 
	  //執行,傳入之sql為完整字串 
	  private ResultSet rs = null; 
	  //結果集 
	  private PreparedStatement pst = null; 
	  //執行,傳入之sql為預儲之字申,需要傳入變數之位置 
	  //先利用?來做標示 
	  	  
	  public DBC() 
	  { 
	    try { 
	      Class.forName("com.mysql.jdbc.Driver"); 
	      //註冊driver 
	      con = DriverManager.getConnection( 
	      "jdbc:mysql://localhost/test?useUnicode=true&characterEncoding=utf8", 
	      "root","12345"); 
	      //取得connection
	      
	    } 
	    catch(ClassNotFoundException e) 
	    { 
	      System.out.println("DriverClassNotFound :"+e.toString()); 
	    }//有可能會產生sqlexception 
	    catch(SQLException x) { 
	      System.out.println("Exception :"+x.toString()); 
	    } 
	    
	  } 
	  
	  public void insertTable( String name,String passwd) 
	  { 
	    try 
	    { 
	      pst = con.prepareStatement(""); 
	   
	      pst.setString(1, name); 
	      pst.setString(2, passwd); 
	      pst.executeUpdate(); 
	    } 
	    catch(SQLException e) 
	    { 
	      System.out.println("InsertDB Exception :" + e.toString()); 
	    } 
	    finally 
	    { 
	      Close(); 
	    } 
	    
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
	  	  
	  
	  
}
