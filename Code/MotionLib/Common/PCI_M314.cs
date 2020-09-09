using System;
using System.Text;
using System.Runtime.InteropServices;

namespace PCI_M314
{
    public class CPCI_M314
    {
        //Initial
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_open")]
        public static extern short CS_m314_open(ref ushort existcard);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_close")]
        public static extern short CS_m314_close(ushort CardNo);

        //Card Operate
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_initial_card")]
        public static extern short CS_m314_initial_card(ushort CardNo);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_reset_card")]
        public static extern short CS_m314_reset_card(ushort CardNo);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_check_dsp_running")]
        public static extern short CS_m314_check_dsp_running(ushort CardNo, ref ushort running);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_buffer_enable")]
        public static extern short CS_m314_buffer_enable(ushort CardNo, ushort Enable);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_cardno")]
        public static extern short CS_m314_get_cardno(ushort seq, ref ushort CardNo);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_version")]
        public static extern short CS_m314_get_version(ushort CardNo, ref ushort ver);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_config_from_file")]
        public static extern short CS_m314_config_from_file(ushort CardNo, string file_name);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_buffer_length")]
        public static extern short CS_m314_get_buffer_length(ushort CardNo, ushort AxisNo, ref ushort bufferLength);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_cardVer")]
        public static extern short CS_m314_get_cardVer(ushort CardNo, ref ushort ver);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_firmwarever")]
        public static extern short CS_m314_get_firmwarever(ushort CardNo, ref ushort ver);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_motion_disable")]
        public static extern short CS_m314_set_motion_disable(ushort CardNo, ushort on_off);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_buffer_clear")]
        public static extern short CS_m314_buffer_clear(ushort CardNo, ushort AxisNo);
        //0:1.25ms,1:2.5ms,2:5ms,3:10ms
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_position_cycle")]
        public static extern short CS_m314_set_position_cycle(ushort CardNo, ushort value);

        //Motion Interface I/O
        ///////SERVO IO 
        //CONFIGURATION
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_alm")]
        public static extern short CS_m314_set_alm(ushort CardNo, ushort AxisNo, short alm_logic, short alm_mode);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_inp")]
        public static extern short CS_m314_set_inp(ushort CardNo, ushort AxisNo, short inp_enable, short inp_logic);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_erc")]
        public static extern short CS_m314_set_erc(ushort CardNo, ushort AxisNo, short erc_on_time);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_servo")]
        public static extern short CS_m314_set_servo(ushort CardNo, ushort AxisNo, short on_off);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_sd")]
        public static extern short CS_m314_set_sd(ushort CardNo, ushort AxisNo, short enable, short sd_logic, short sd_mode);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_ralm")]
        public static extern short CS_m314_set_ralm(ushort CardNo, ushort AxisNo, short on_off);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_erc_on")]
        public static extern short CS_m314_set_erc_on(ushort CardNo, ushort AxisNo, short on_off);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_ell")]
        public static extern short CS_m314_set_ell(ushort CardNo, ushort AxisNo, short ell_logic);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_org")]
        public static extern short CS_m314_set_org(ushort CardNo, ushort AxisNo, short org_logic);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_emg")]
        public static extern short CS_m314_set_emg(ushort CardNo, ushort AxisNo, short emg_logic);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_ez")]
        public static extern short CS_m314_set_ez(ushort CardNo, ushort AxisNo, short ez_logic);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_ltc_logic")]
        public static extern short CS_m314_set_ltc_logic(ushort CardNo, ushort AxisNo, short ltc_logic);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_ltc_src")]
        public static extern short CS_m314_set_ltc_src(ushort CardNo, ushort AxisNo, short ltc_src);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_ltc_src")]
        public static extern short CS_m314_get_ltc_src(ushort CardNo, ushort AxisNo, ref short ltc_src);
        //Motion status
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_motion_done")]
        public static extern short CS_m314_motion_done(ushort CardNo, ushort AxisNo, ref ushort MC_status);
        //Motion IO Monitor
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_io_status")]
        public static extern short CS_m314_get_io_status(ushort CardNo, ushort AxisNo, ref ushort io_sts);
        //Motion P Command Control
        //{
        //Motion Velocity mode
        //Motion Single Axis
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_tr_move")]
        public static extern short CS_m314_start_tr_move(ushort CardNo, ushort AxisNo, int Dist, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_ta_move")]
        public static extern short CS_m314_start_ta_move(ushort CardNo, ushort AxisNo, int Dist, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sr_move")]
        public static extern short CS_m314_start_sr_move(ushort CardNo, ushort AxisNo, int Dist, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sa_move")]
        public static extern short CS_m314_start_sa_move(ushort CardNo, ushort AxisNo, int Dist, int StrVel, int MaxVel, double Tacc, double Tdec);
        //Motion Multi Axes
        //2 Axes Linear move
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_tr_move_xy")]
        public static extern short CS_m314_start_tr_move_xy(ushort CardNo, ref ushort AxisArray, int DisX, int DisY, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sr_move_xy")]
        public static extern short CS_m314_start_sr_move_xy(ushort CardNo, ref ushort AxisArray, int DisX, int DisY, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_ta_move_xy")]
        public static extern short CS_m314_start_ta_move_xy(ushort CardNo, ref ushort AxisArray, int DisX, int DisY, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sa_move_xy")]
        public static extern short CS_m314_start_sa_move_xy(ushort CardNo, ref ushort AxisArray, int DisX, int DisY, int StrVel, int MaxVel, double Tacc, double Tdec);
        //2 Axes Circle Move
        //Angle>0:CW..Angle<0:CCW   //Center Point ,Angle
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_tr_arc_xy")]
        public static extern short CS_m314_start_tr_arc_xy(ushort CardNo, ref ushort AxisArray, int Center_X, int Center_Y, double Angle, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sr_arc_xy")]
        public static extern short CS_m314_start_sr_arc_xy(ushort CardNo, ref ushort AxisArray, int Center_X, int Center_Y, double Angle, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_ta_arc_xy")]
        public static extern short CS_m314_start_ta_arc_xy(ushort CardNo, ref ushort AxisArray, int Center_X, int Center_Y, double Angle, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sa_arc_xy")]
        public static extern short CS_m314_start_sa_arc_xy(ushort CardNo, ref ushort AxisArray, int Center_X, int Center_Y, double Angle, int StrVel, int MaxVel, double Tacc, double Tdec);
        //Angle>0:CW..Angle<0:CCW   //End Point ,Angle
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_tr_arc2_xy")]
        public static extern short CS_m314_start_tr_arc2_xy(ushort CardNo, ref ushort AxisArray, int End_X, int End_Y, double Angle, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sr_arc2_xy")]
        public static extern short CS_m314_start_sr_arc2_xy(ushort CardNo, ref ushort AxisArray, int End_X, int End_Y, double Angle, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_ta_arc2_xy")]
        public static extern short CS_m314_start_ta_arc2_xy(ushort CardNo, ref ushort AxisArray, int End_X, int End_Y, double Angle, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sa_arc2_xy")]
        public static extern short CS_m314_start_sa_arc2_xy(ushort CardNo, ref ushort AxisArray, int End_X, int End_Y, double Angle, int StrVel, int MaxVel, double Tacc, double Tdec);
        //Dir=1:CW..Dir=0:CCW   //End Point ,Center Point
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_tr_arc3_xy")]
        public static extern short CS_m314_start_tr_arc3_xy(ushort CardNo, ref ushort AxisArray, int Center_X, int Center_Y, int End_X, int End_Y, short Dir, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sr_arc3_xy")]
        public static extern short CS_m314_start_sr_arc3_xy(ushort CardNo, ref ushort AxisArray, int Center_X, int Center_Y, int End_X, int End_Y, short Dir, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_ta_arc3_xy")]
        public static extern short CS_m314_start_ta_arc3_xy(ushort CardNo, ref ushort AxisArray, int Center_X, int Center_Y, int End_X, int End_Y, short Dir, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sa_arc3_xy")]
        public static extern short CS_m314_start_sa_arc3_xy(ushort CardNo, ref ushort AxisArray, int Center_X, int Center_Y, int End_X, int End_Y, short Dir, int StrVel, int MaxVel, double Tacc, double Tdec);
        //3 Axes Linear move
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_tr_move_xyz")]
        public static extern short CS_m314_start_tr_move_xyz(ushort CardNo, ref ushort AxisArray, int DisX, int DisY, int DisZ, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sr_move_xyz")]
        public static extern short CS_m314_start_sr_move_xyz(ushort CardNo, ref ushort AxisArray, int DisX, int DisY, int DisZ, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_ta_move_xyz")]
        public static extern short CS_m314_start_ta_move_xyz(ushort CardNo, ref ushort AxisArray, int DisX, int DisY, int DisZ, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sa_move_xyz")]
        public static extern short CS_m314_start_sa_move_xyz(ushort CardNo, ref ushort AxisArray, int DisX, int DisY, int DisZ, int StrVel, int MaxVel, double Tacc, double Tdec);
        //3 Axes Heli move Dir=1:CW..Dir=0:CCW
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_tr_heli_xy")]
        public static extern short CS_m314_start_tr_heli_xy(ushort CardNo, ref ushort AxisArray, int Center_X, int Center_Y, int Depth, int Pitch, short Dir, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sr_heli_xy")]
        public static extern short CS_m314_start_sr_heli_xy(ushort CardNo, ref ushort AxisArray, int Center_X, int Center_Y, int Depth, int Pitch, short Dir, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_ta_heli_xy")]
        public static extern short CS_m314_start_ta_heli_xy(ushort CardNo, ref ushort AxisArray, int Center_X, int Center_Y, int Depth, int Pitch, short Dir, int StrVel, int MaxVel, double Tacc, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_sa_heli_xy")]
        public static extern short CS_m314_start_sa_heli_xy(ushort CardNo, ref ushort AxisArray, int Center_X, int Center_Y, int Depth, int Pitch, short Dir, int StrVel, int MaxVel, double Tacc, double Tdec);
        //}	//Motion Multi Axes

        //Motion Stop
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_emg_stop")]
        public static extern short CS_m314_emg_stop(ushort CardNo, ushort AxisNo);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_emg_stop_erc")]
        public static extern short CS_m314_emg_stop_erc(ushort CardNo, ushort AxisNo);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_sd_stop")]
        public static extern short CS_m314_sd_stop(ushort CardNo, ushort AxisNo, double Tdec);
        ///////HOMING 
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_home_config")]
        public static extern short CS_m314_set_home_config(ushort CardNo, ushort AxisNo, short home_mode, short org_logic, short ez_logic, short ez_count);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_home_move")]
        public static extern short CS_m314_home_move(ushort CardNo, ushort AxisNo, int StrVel, int MaxVel, double Tacc, short Dir);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_disable_home_move")]
        public static extern short CS_m314_disable_home_move(ushort CardNo, ushort AxisNo, double Tdec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_home_offset_position")]
        public static extern short CS_m314_set_home_offset_position(ushort CardNo, ushort AxisNo, int pos);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_home_finish_reset")]
        public static extern short CS_m314_set_home_finish_reset(ushort CardNo, ushort AxisNo, ushort enable);

        //Motion Counter Operating
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_ltc_position")]
        public static extern short CS_m314_get_ltc_position(ushort CardNo, ushort AxisNo, ref double ltc_pos);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_ltc_position_manual_clr")]
        public static extern short CS_m314_get_ltc_position_manual_clr(ushort CardNo, ushort AxisNo, ref double ltc_pos, ushort clr);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_current_speed")]
        public static extern short CS_m314_get_current_speed(ushort CardNo, ushort AxisNo, ref double speed);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_position")]
        public static extern short CS_m314_get_position(ushort CardNo, ushort AxisNo, ref double pos);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_position")]
        public static extern short CS_m314_set_position(ushort CardNo, ushort AxisNo, double pos);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_command")]
        public static extern short CS_m314_get_command(ushort CardNo, ushort AxisNo, ref int cmd);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_command")]
        public static extern short CS_m314_set_command(ushort CardNo, ushort AxisNo, int cmd);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_move_ratio")]
        public static extern short CS_m314_set_move_ratio(ushort CardNo, ushort AxisNo, double move_ratio);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_electronic_cam")]
        public static extern short CS_m314_set_electronic_cam(ushort CardNo, ushort AxisNo, short numerator, short denominator, ushort Enable);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_gear")]
        public static extern short CS_m314_set_gear(ushort CardNo, ushort AxisNo, short numerator, short denominator, ushort Enable);

        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_error_counter")]
        public static extern short CS_m314_get_error_counter(ushort CardNo, ushort AxisNo, ref short error);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_reset_error_counter")]
        public static extern short CS_m314_reset_error_counter(ushort CardNo, ushort AxisNo);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_target_pos")]
        public static extern short CS_m314_get_target_pos(ushort CardNo, ushort AxisNo, ref double pos);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_p_change")]
        public static extern short CS_m314_p_change(ushort CardNo, ushort AxisNo, int NewPos);

        //Position Compare
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_position_cmp")]
        public static extern short CS_m314_position_cmp(ushort CardNo, ushort Comparechannel, int start, int end, uint interval);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_position_cmp_table")]
        public static extern short CS_m314_position_cmp_table(ushort CardNo, ushort Comparechannel, int[] TriggerTable, int count, int offset);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_position_cmp_level")]
        public static extern short CS_m314_position_cmp_level(ushort CardNo, ushort Comparechannel, int start, int end, uint interval, ushort first_level_on_off);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_position_cmp_table_level")]
        public static extern short CS_m314_position_cmp_table_level(ushort CardNo, ushort Comparechannel, int[] TriggerTable, int count, int offset, ushort first_level_on_off);

        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_trigger_enable")]
        public static extern short CS_m314_set_trigger_enable(ushort CardNo, ushort Comparechannel, ushort enable);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_trigger_src")]
        public static extern short CS_m314_set_trigger_src(ushort CardNo, ushort Comparechannel, ushort CompareSrc, ushort OutputSrc, ushort OupPulseWidth);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_trigger_cnt")]
        public static extern short CS_m314_get_trigger_cnt(ushort CardNo, ushort Comparechannel, ref int trigger_cnt);

        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_cmp_oneshut")]
        public static extern short CS_m314_cmp_oneshut(ushort CardNo, ushort Comparechannel);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_cmp_gpio")]
        public static extern short CS_m314_cmp_gpio(ushort CardNo, ushort Comparechannel, ushort on_off);


        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_position_cmp_high_speed")]
        public static extern short CS_m314_position_cmp_high_speed(ushort CardNo, ushort Comparechannel, int start, ushort dir, ushort interval, uint trigger_cnt);


        ///VELOCITY MOVE
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_tv_move")]
        public static extern short CS_m314_tv_move(ushort CardNo, ushort AxisNo, int StrVel, int MaxVel, double Tacc, short Dir);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_sv_move")]
        public static extern short CS_m314_sv_move(ushort CardNo, ushort AxisNo, int StrVel, int MaxVel, double Tacc, short Dir);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_v_change")]
        public static extern short CS_m314_v_change(ushort CardNo, ushort AxisNo, int NewVel, double Time);
        //Pulse Input/Output Configuration
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_pls_outmode")]
        public static extern short CS_m314_set_pls_outmode(ushort CardNo, ushort AxisNo, short pls_outmode);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_pls_outfastmode")]
        public static extern short CS_m314_set_pls_outfastmode(ushort CardNo, ushort AxisNo, short enable);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_pls_outwidth")]
        public static extern short CS_m314_set_pls_outwidth(ushort CardNo, ushort AxisNo, short pls_outwidth);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_pls_iptmode")]
        public static extern short CS_m314_set_pls_iptmode(ushort CardNo, ushort AxisNo, short inputMode, short pls_logic);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_feedback_src")]
        public static extern short CS_m314_set_feedback_src(ushort CardNo, ushort AxisNo, short Src);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_a_move_feedback_src")]
        public static extern short CS_m314_set_a_move_feedback_src(ushort CardNo, ushort AxisNo, short Src);
        //INTERRUPT 
        public delegate void PM314UserCbk(UInt16 CardNo, UInt16 AxisNo);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_link_interrupt")]
        public static extern short CS_m314_link_interrupt(ushort CardNo, PM314UserCbk callback);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_int_factor")]
        public static extern short CS_m314_set_int_factor(ushort CardNo, ushort AxisNo, ushort int_factor);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_int_enable")]
        public static extern short CS_m314_int_enable(ushort CardNo, ushort AxisNo);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_int_disable")]
        public static extern short CS_m314_int_disable(ushort CardNo, ushort AxisNo);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_int_status")]
        public static extern short CS_m314_get_int_status(ushort CardNo, ushort AxisNo, ref ushort event_int_status);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_int_count")]
        public static extern short CS_m314_get_int_count(ushort CardNo, ushort AxisNo, ref ushort count);
        ////////SOFT LIMIT
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_soft_limit")]
        public static extern short CS_m314_set_soft_limit(ushort CardNo, ushort AxisNo, int PLimit, int NLimit);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_enable_soft_limit")]
        public static extern short CS_m314_enable_soft_limit(ushort CardNo, ushort AxisNo, short Action);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_disable_soft_limit")]
        public static extern short CS_m314_disable_soft_limit(ushort CardNo, ushort AxisNo);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_soft_limit_status")]
        public static extern short CS_m314_get_soft_limit_status(ushort CardNo, ushort AxisNo, ref ushort PLimit_sts, ref ushort NLimit_sts);

        ////////DIO 
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_dio_output")]
        public static extern short CS_m314_set_dio_output(ushort CardNo, ushort AxisNo, ushort output);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_dio_input")]
        public static extern short CS_m314_set_dio_input(ushort CardNo, ushort AxisNo, ref ushort input);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_dio_input")]
        public static extern short CS_m314_get_dio_input(ushort CardNo, ushort AxisNo, ref ushort input);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_dio_output")]
        public static extern short CS_m314_get_dio_output(ushort CardNo, ushort AxisNo, ref ushort output);
        //MISC 
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_dwell_buffer")]
        public static extern short CS_m314_dwell_buffer(ushort CardNo, ushort AxisNo, uint TimeCnt);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_interrupt_buffer")]
        public static extern short CS_m314_set_interrupt_buffer(ushort CardNo, ushort AxisNo);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_compare_int")]
        public static extern short CS_m314_set_compare_int(ushort CardNo, ushort AxisNo, ref int table, ushort total_cnt, ushort compare_dir);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_compare_count")]
        public static extern short CS_m314_get_compare_count(ushort CardNo, ushort AxisNo, ref ushort index);


        [DllImport("PCI_M314_X64.dll", EntryPoint = "_misc_app_get_circle_endpoint")]
        public static extern short CS_misc_app_get_circle_endpoint(int Start_X, int Start_Y, int Center_X, int Center_Y, double Angle, ref int end_x, ref int end_y);

        //Trace
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_enable_tracing_axis")]
        public static extern short CS_m314_enable_tracing_axis(ushort CardNo, ref ushort AxisNo, ushort enable);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_monitor_tracing_axis")]
        public static extern short CS_m314_monitor_tracing_axis(ushort CardNo, ushort AxisNo, ushort mon_enable, ushort mon_type, int pos_err);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_tracing_axis_lag")]
        public static extern short CS_m314_get_tracing_axis_lag(ushort CardNo, ushort AxisNo, ref int pos_lag);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_tracing_passing_timer")]
        public static extern short CS_m314_set_tracing_passing_timer(ushort CardNo, ushort AxisNo, ushort timer);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_trace_rate")]
        public static extern short CS_m314_set_trace_rate(ushort CardNo, ushort AxisNo, ushort numerator, ushort denominator);
        //SYNC Move
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_sync_move")]
        public static extern short CS_m314_sync_move(short CardNo);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_sync_move_config")]
        public static extern short CS_m314_sync_move_config(short CardNo, short AxisNo, short enable);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_motion_dio_sync_start")]
        public static extern short CS_m314_set_motion_dio_sync_start(short CardNo, short AxisNo, ushort enable);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_premove_config")]
        public static extern short CS_m314_set_premove_config(short CardNo, short AxisNo, short enable, short src_axis, short dir, int pos);

        //dda go
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_dda_table")]
        public static extern short CS_m314_dda_table(ushort CardNo, ushort AxisNo, ref short dda_table, int count, int offset);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_dda_enable")]
        public static extern short CS_m314_dda_enable(ushort CardNo, ref ushort Axis_dda_active, ushort enable);
        ////////Special DIO for COMWEB
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_dio_output")]
        public static extern short CS_m314_dio_output(ushort CardNo, ushort AxisNo, ushort output);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_dio_input")]
        public static extern short CS_m314_dio_input(ushort CardNo, ushort AxisNo, ref ushort input);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_dda_from_file")]
        public static extern short CS_m314_dda_from_file(ushort CardNo, ushort AxisNo, string file_name);


        //20080918
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_cam_param")]
        public static extern short CS_m314_set_cam_param(ushort CardNo, int xOriginalPos, int xPitch, ref int camTable, int camTableLen);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_enable_cam_func")]
        public static extern short CS_m314_enable_cam_func(ushort CardNo, ushort AxisNo, ushort enable);


        //Feed Hold
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_feedhold_stop")]
        public static extern short CS_m314_feedhold_stop(ushort CardNo, ushort AxisNo, double Tdec, ushort On_off);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_feedhold_enable")]
        public static extern short CS_m314_feedhold_enable(ushort CardNo, ushort enable);


        //Software IO set record position
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_io_record_position_cfg")]
        public static extern short CS_m314_set_io_record_position_cfg(ushort CardNo, ushort DIchannel, ushort axis_pos, ushort polarity, ushort filter_time, ushort enable);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_io_record_position_cnt")]
        public static extern short CS_m314_get_io_record_position_cnt(ushort CardNo, ushort DIchannel, ref ushort cnt);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_io_record_position")]
        public static extern short CS_m314_get_io_record_position(ushort CardNo, ushort DIchannel, ushort Index, ref int pos);


        //Muliti Axis Function
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_multi_axes_move")]
        public static extern short CS_m314_start_multi_axes_move(ushort CardNo, ushort AxisNum, ref ushort AxisNo, ref int DistArrary, int StrVel, int MaxVel, double Tacc, double Tdec, ushort m_curve, ushort m_r_a);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_spiral_xy")]
        public static extern short CS_m314_start_spiral_xy(ushort CardNo, ref ushort AxisNo, int Center_X, int Center_Y, int spiral_interval, uint spiral_angle, int StrVel, int MaxVel, double Tacc, double Tdec, ushort m_curve, ushort m_r_a);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_spiral2_xy")]
        public static extern short CS_m314_start_spiral2_xy(ushort CardNo, ref ushort AxisNo, int center_x, int center_y, int end_x, int end_y, ushort dir, ushort circlenum, int StrVel, int MaxVel, double Tacc, double Tdec, ushort m_curve, ushort m_r_a);



        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_v3_move")]
        public static extern short CS_m314_start_v3_move(ushort CardNo, ushort AxisNo, int Dist, int StrVel, int MaxVel, int EndVel, double Tacc, double Tdec, ushort m_curve, ushort m_r_a);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_v3_move_xy")]
        public static extern short CS_m314_start_v3_move_xy(ushort CardNo, ref ushort AxisNo, int DisX, int DisY, int StrVel, int MaxVel, int EndVel, double Tacc, double Tdec, ushort m_curve, ushort m_r_a);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_v3_arc_xy")]
        public static extern short CS_m314_start_v3_arc_xy(ushort CardNo, ref ushort AxisNo, int Center_X, int Center_Y, double Angle, int StrVel, int MaxVel, int EndVel, double Tacc, double Tdec, ushort m_curve, ushort m_r_a);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_v3_arc2_xy")]
        public static extern short CS_m314_start_v3_arc2_xy(ushort CardNo, ref ushort AxisNo, int End_X, int End_Y, double Angle, int StrVel, int MaxVel, int EndVel, double Tacc, double Tdec, ushort m_curve, ushort m_r_a);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_v3_arc3_xy")]
        public static extern short CS_m314_start_v3_arc3_xy(ushort CardNo, ref ushort AxisNo, int Center_X, int Center_Y, int End_x, int End_y, short Dir, int StrVel, int MaxVel, int EndVel, double Tacc, double Tdec, ushort m_curve, ushort m_r_a);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_v3_move_xyz")]
        public static extern short CS_m314_start_v3_move_xyz(ushort CardNo, ref ushort AxisNo, int DisX, int DisY, int DisZ, int StrVel, int MaxVel, int EndVel, double Tacc, double Tdec, ushort m_curve, ushort m_r_a);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_v3_heli_xy")]
        public static extern short CS_m314_start_v3_heli_xy(ushort CardNo, ref ushort AxisNo, int Center_X, int Center_Y, int Depth, int Pitch, short Dir, int StrVel, int MaxVel, int EndVel, double Tacc, double Tdec, ushort m_curve, ushort m_r_a);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_v3_multi_axes")]
        public static extern short CS_m314_start_v3_multi_axes(ushort CardNo, ushort AxisNum, ref ushort AxisNo, ref int DistArrary, int StrVel, int MaxVel, int EndVel, double Tacc, double Tdec, ushort m_curve, ushort m_r_a);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_v3_spiral_xy")]
        public static extern short CS_m314_start_v3_spiral_xy(ushort CardNo, ref ushort AxisNo, int Center_X, int Center_Y, int spiral_interval, uint spiral_angle, int StrVel, int MaxVel, int EndVel, double Tacc, double Tdec, ushort m_curve, ushort m_r_a);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_start_v3_spiral2_xy")]
        public static extern short CS_m314_start_v3_spiral2_xy(ushort CardNo, ref ushort AxisNo, int center_x, int center_y, int end_x, int end_y, ushort dir, ushort circlenum, int StrVel, int MaxVel, int EndVel, double Tacc, double Tdec, ushort m_curve, ushort m_r_a);


        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_scurve_rate")]
        public static extern short CS_m314_set_scurve_rate(ushort CardNo, ushort AxisNo, ushort scurve_rate);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_feedrate_overwrite")]
        public static extern short CS_m314_set_feedrate_overwrite(ushort CardNo, ushort AxisNo, ushort Mode, int New_Speed, double sec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_sd_mode")]
        public static extern short CS_m314_set_sd_mode(ushort CardNo, ushort AxisNo, ushort mode);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_sd_time")]
        public static extern short CS_m314_set_sd_time(ushort CardNo, ushort AxisNo, double sd_dec);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_DLL_path")]
        public static extern short CS_m314_get_DLL_path(byte[] lpFilePath, uint nSize, ref uint nLength);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_DLL_version")]
        public static extern short CS_m314_get_DLL_version(byte[] lpBuf, uint nSize, ref uint nLength);

        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_ini_from_file")]
        public static extern short CS_m314_ini_from_file(ushort CardNo, string file_name);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_Monitor_Counter")]
        public static extern short CS_m314_Monitor_Counter(ushort CardNo, ushort AxisNo, ref ushort DSP_Cnt, ref ushort PC_Cnt);

        //20130204
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_backlash")]
        public static extern short CS_m314_set_backlash(short CardNo, ushort AxisNo, short enable);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_backlash_info")]
        public static extern short CS_m314_set_backlash_info(short CardNo, ushort AxisNo, short backlash, ushort accstep, ushort contstep, ushort decstep);

        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_pitcherr_pitch")]
        public static extern short CS_m314_set_pitcherr_pitch(ushort CardNo, ushort AxisNo, int pitch);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_pitcherr_org")]
        public static extern short CS_m314_set_pitcherr_org(short CardNo, ushort AxisNo, short Dir, int orgpos);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_pitcherr_enable")]
        public static extern short CS_m314_set_pitcherr_enable(short CardNo, ushort AxisNo, ushort on_off);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_pitcherr_mode")]
        public static extern short CS_m314_set_pitcherr_mode(short CardNo, ushort AxisNo, ushort Mode);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_pitcherr_table")]
        public static extern short CS_m314_set_pitcherr_table(short CardNo, ushort AxisNo, short Dir, ref int table);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_pitcherr_table2")]
        public static extern short CS_m314_set_pitcherr_table2(short CardNo, ushort AxisNo, short Dir, ref int table, int Num);
        //20130205
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_enable_electcam")]
        public static extern short CS_m314_enable_electcam(short CardNo, ushort AxisNo, ushort enable, ushort axisbit, ushort mode);
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_set_monitor_tracing_lag_offset")]
        public static extern short CS_m314_set_monitor_tracing_lag_offset(ushort CardNo, ushort AxisNo, int lag_offset);
        //20130325
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_gear")]
        public static extern short CS_m314_get_gear(ushort CardNo, ushort AxisNo, ref short numerator, ref short denominator, ref ushort Enable);

        //20130411
        [DllImport("PCI_M314_X64.dll", EntryPoint = "_m314_get_servo_command")]
        public static extern short CS_m314_get_servo_command(ushort CardNo, ushort AxisNo, ref int cmd);

    }
}
