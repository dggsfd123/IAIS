package test;

import javax.inject.Inject;
import javax.inject.Named;


import com.kuka.common.ThreadUtil;
import com.kuka.generated.ioAccess.MediaFlangeIOGroup;
import com.kuka.roboticsAPI.applicationModel.RoboticsAPIApplication;
import static com.kuka.roboticsAPI.motionModel.BasicMotions.*;

import com.kuka.roboticsAPI.deviceModel.LBR;
import com.kuka.roboticsAPI.geometricModel.Tool;
import com.kuka.task.ITaskLogger;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.PrintWriter;
import java.net.Socket;
import java.net.UnknownHostException;
import java.io.InputStream;
import java.io.OutputStream;

/**
 * Implementation of a robot application.
 * <p>
 * The application provides a {@link RoboticsAPITask#initialize()} and a
 * {@link RoboticsAPITask#run()} method, which will be called successively in
 * the application lifecycle. The application will terminate automatically after
 * the {@link RoboticsAPITask#run()} method has finished or after stopping the
 * task. The {@link RoboticsAPITask#dispose()} method will be called, even if an
 * exception is thrown during initialization or run.
 * <p>
 * <b>It is imperative to call <code>super.dispose()</code> when overriding the
 * {@link RoboticsAPITask#dispose()} method.</b>
 * 
 * @see UseRoboticsAPIContext
 * @see #initialize()
 * @see #run()
 * @see #dispose()
 */
public class Animal extends RoboticsAPIApplication {
	@Inject
	private LBR lbr;

	@Inject
	private ITaskLogger logger;

	
	@Inject
	@Named("Gripper")
	private Tool robTool;
	
	@Inject
	private MediaFlangeIOGroup ioGroup;
	
	
	@Override
	public void initialize() {
		// initialize your application here
		logger.info("Initialize");
		robTool.attachTo(lbr.getFlange());
		gripperClose();
		lbr.move(ptp(getApplicationData().getFrame("/MoveFox")));
	}

	
	
	@Override
	public void run() {
		// your application execution starts here
		
		int times = 20;
		while(times>0){
			try {
				Socket socket=new Socket("172.31.1.140",5877);
				InputStream is = socket.getInputStream();
				BufferedReader br=new BufferedReader(new InputStreamReader(is));
				OutputStream os=socket.getOutputStream();
				PrintWriter pw=new PrintWriter(os);	
				pw.write("ready");
				pw.flush();		
				String info="1";
				info=br.readLine();			
				logger.info("get message "+info);
				
				
				//Move Static
				if (info.equals("11")) {
					logger.info("11");
					lbr.move(ptp(getApplicationData().getFrame("/P2")));
				}
				else if (info.equals("22")) {
					logger.info("22");
					lbr.move(ptp(getApplicationData().getFrame("/P3")));
				}
				else if (info.equals("33")) {
					logger.info("33");
					lbr.move(ptp(getApplicationData().getFrame("/P2")));
				}
				else if (info.equals("44")) {
					logger.info("44");
					lbr.move(lin(getApplicationData().getFrame("/MoveFox/P44")).setCartVelocity(100));
				}
				else if (info.equals("55")) {
					logger.info("55");
					lbr.move(lin(getApplicationData().getFrame("/MoveFox/P55")).setCartVelocity(100));
				}
				else if (info.equals("66")) {
					logger.info("66");
					lbr.move(lin(getApplicationData().getFrame("/MoveFox/P66")).setCartVelocity(100));
				}
				else if (info.equals("77")) {
					logger.info("77");
					lbr.move(ptp(getApplicationData().getFrame("/P4")));
				}
				else if (info.equals("88")) {
					logger.info("88");
					lbr.move(ptp(getApplicationData().getFrame("/P5")));
				}
				else if (info.equals("99")) {
					logger.info("99");
					lbr.move(ptp(getApplicationData().getFrame("/P4")));
				}				
				
				//Idle
				else if (info.equals("id")) {
					logger.info("Idle");
					lbr.move(lin(getApplicationData().getFrame("/Pet/aIdle")).setCartVelocity(100));
				}
				
				//Attack
				else if (info.equals("sa")) {
					logger.info("SmallAttack");
					lbr.move(lin(getApplicationData().getFrame("/Pet/bAttackSmall")).setCartVelocity(100));
					lbr.move(lin(getApplicationData().getFrame("/Pet/aIdle")).setCartVelocity(100));
				}
				else if (info.equals("ba")) {
					logger.info("BigAttack");
					lbr.move(lin(getApplicationData().getFrame("/Pet/cAttackBig")).setCartVelocity(100));
					lbr.move(lin(getApplicationData().getFrame("/Pet/aIdle")).setCartVelocity(100));
				}
				
				//Food
				else if (info.equals("sf")) {
					logger.info("SmallEat");
					lbr.move(lin(getApplicationData().getFrame("/Pet/fEat")).setCartVelocity(100));	
					lbr.move(lin(getApplicationData().getFrame("/Pet/aIdle")).setCartVelocity(100));
				}
				else if (info.equals("bf")) {
					logger.info("BigEat");
					lbr.move(lin(getApplicationData().getFrame("/Pet/kEatBig")).setCartVelocity(100));
					lbr.move(lin(getApplicationData().getFrame("/Pet/aIdle")).setCartVelocity(100));
				}
				
				//Jump
				else if (info.equals("bj")) {
					logger.info("BigJump");
					lbr.move(lin(getApplicationData().getFrame("/Pet/eJumpNew")).setCartVelocity(100));
					lbr.move(lin(getApplicationData().getFrame("/Pet/aIdle")).setCartVelocity(100));
				}
				else if (info.equals("sj")) {
					logger.info("SmallJump");
					lbr.move(lin(getApplicationData().getFrame("/Pet/dJumpSmall")).setCartVelocity(100));
					lbr.move(lin(getApplicationData().getFrame("/Pet/aIdle")).setCartVelocity(100));
				}
				
				//Exit
				else if (info.equals("ee")) {
					logger.info("Exit");
					lbr.move(lin(getApplicationData().getFrame("/Pet/aIdle")).setCartVelocity(100));
					times = 0;
					break;
				}
				else {
					logger.info("uncorrect message");
					lbr.move(lin(getApplicationData().getFrame("/Pet/aIdle")).setCartVelocity(100));
				}
				br.close();
				is.close();
				pw.close();
				os.close();
				socket.close();
				} catch (UnknownHostException e) {
					e.printStackTrace();
				} catch (IOException e) {
					e.printStackTrace();
				}
			times = times - 1;
			}
	}

	@Override
	public void dispose() {
		robTool.detach();
	}

	private void gripperClose(){
		ioGroup.setOutput1(false);
		ioGroup.setOutput2(true);
		ThreadUtil.milliSleep(2000);
	}
}