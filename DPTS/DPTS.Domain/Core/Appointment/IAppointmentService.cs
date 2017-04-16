using System.Collections.Generic;
using DPTS.Domain.Entities;

namespace DPTS.Domain.Core.Appointment
{
    /// <summary>
    /// Appointment service interface
    /// </summary>
    public interface IAppointmentService
    {
        #region Appoinment Schedule

        /// <summary>
        /// Deletes a Appointment
        /// </summary>
        /// <param name="schedule">Appointment</param>
        void DeleteAppointmentSchedule(AppointmentSchedule schedule);

        /// <summary>
        /// Gets all Appointment Schedule
        /// </summary>
        /// <returns>AppointmentSchedule</returns>
        IList<AppointmentSchedule> GetAllAppointmentSchedule();

        /// <summary>
        /// Gets a AppointmentSchedule
        /// </summary>
        /// <param name="scheduleId">AppointmentSchedule identifier</param>
        /// <returns>AppointmentSchedule</returns>
        AppointmentSchedule GetAppointmentScheduleById(int scheduleId);

        /// <summary>
        /// Get Appointment Schedules
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        IList<AppointmentSchedule> GetAppointmentScheduleByDoctorId(string doctorId);

        /// <summary>
        /// Get Appointment Schedules by patient id
        /// </summary>
        /// <param name="patientId"></param>
        /// <returns></returns>
        IList<AppointmentSchedule> GetAppointmentScheduleByPatientId(string patientId);

        /// <summary>
        /// Get Appointment Schedule by identifiers
        /// </summary>
        /// <param name="scheduleIds">Country identifiers</param>
        /// <returns>Countries</returns>
        IList<AppointmentSchedule> GetAppointmentScheduleByIds(int[] scheduleIds);

        /// <summary>
        /// Inserts a Appointment Schedule
        /// </summary>
        /// <param name="schedule">Country</param>
        void InsertAppointmentSchedule(AppointmentSchedule schedule);

        /// <summary>
        /// Updates the Appointment Schedule
        /// </summary>
        /// <param name="schedule">Country</param>
        void UpdateAppointmentSchedule(AppointmentSchedule schedule);
        #endregion

        #region Doctor Schedule

        /// <summary>
        /// Deletes a schedule
        /// </summary>
        /// <param name="schedule">Appointment</param>
        void DeleteSchedule(Schedule schedule);

        /// <summary>
        /// Gets all Schedule
        /// </summary>
        /// <returns>Schedule</returns>
        IList<Schedule> GetAllSchedule();

        /// <summary>
        /// Gets a Schedule
        /// </summary>
        /// <param name="scheduleId">Schedule identifier</param>
        /// <returns>Schedule</returns>
        Schedule GetScheduleById(int scheduleId);

        /// <summary>
        /// Get Appointment Schedules
        /// </summary>
        /// <param name="doctorId"></param>
        /// <returns></returns>
        IList<Schedule> GetScheduleByDoctorId(string doctorId);

        /// <summary>
        /// Get Schedule by identifiers
        /// </summary>
        /// <param name="scheduleIds">Country identifiers</param>
        /// <returns>Countries</returns>
        IList<Schedule> GetScheduleByIds(int[] scheduleIds);

        /// <summary>
        /// Inserts a Schedule
        /// </summary>
        /// <param name="schedule">schedule</param>
        void InsertSchedule(Schedule schedule);

        /// <summary>
        /// Updates the Schedule
        /// </summary>
        /// <param name="schedule">schedule</param>
        void UpdateSchedule(Schedule schedule);
        #endregion

        #region Booking Status

        /// <summary>
        /// get all status
        /// </summary>
        /// <returns></returns>
        IList<AppointmentStatus> GetAllAppointmentStatus();

        /// <summary>
        /// get status by name
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        AppointmentStatus GetAppointmentStatusByName(string status);
        #endregion

    }
}