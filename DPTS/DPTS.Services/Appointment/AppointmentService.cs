using System;
using System.Collections.Generic;
using System.Linq;
using DPTS.Domain.Core;
using DPTS.Domain.Core.Appointment;
using DPTS.Domain.Entities;

namespace DPTS.Domain.Appointment
{
    public class AppointmentService : IAppointmentService
    {
        #region Fields
        private readonly IRepository<AppointmentSchedule> _scheduleRepository;
        private readonly IRepository<AppointmentStatus> _statusRepository;
        private readonly IRepository<Schedule> _docScheduleRepository;
        #endregion

        #region Ctor
        public AppointmentService(IRepository<AppointmentSchedule> scheduleRepository, 
            IRepository<Schedule> docScheduleRepository,
            IRepository<AppointmentStatus> statusRepository)
        {
            _scheduleRepository = scheduleRepository;
            _docScheduleRepository = docScheduleRepository;
            _statusRepository = statusRepository;
        }
        #endregion

        #region Methods
        public void DeleteAppointmentSchedule(AppointmentSchedule schedule)
        {
            if(schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            _scheduleRepository.Delete(schedule);
        }

        public IList<AppointmentSchedule> GetAllAppointmentSchedule()
        {
            var query = _scheduleRepository.Table;
            return query.ToList();
        }

        public AppointmentSchedule GetAppointmentScheduleById(int scheduleId)
        {
            if (scheduleId == 0)
                return null;

            return _scheduleRepository.GetById(scheduleId);
        }

        public IList<AppointmentSchedule> GetAppointmentScheduleByDoctorId(string doctorId)
        {
            if (string.IsNullOrWhiteSpace(doctorId))
                return null;

            var query = from c in _scheduleRepository.Table
                where c.DoctorId.Equals(doctorId)
                select c;

            return query.ToList();
        }
        public IList<AppointmentSchedule> GetAppointmentScheduleByPatientId(string patientId)
        {
            if (string.IsNullOrWhiteSpace(patientId))
                return null;

            var query = from c in _scheduleRepository.Table
                        where c.PatientId.Equals(patientId)
                        select c;

            return query.ToList();
        }

        public IList<AppointmentSchedule> GetAppointmentScheduleByIds(int[] scheduleIds)
        {
            if (scheduleIds == null || scheduleIds.Length == 0)
                return new List<AppointmentSchedule>();

            var query = from c in _scheduleRepository.Table
                where scheduleIds.Contains(c.Id)
                select c;
            var schedules = query.ToList();
            return scheduleIds.Select(id => schedules.Find(x => x.Id == id)).Where(schedule => schedule != null).ToList();
        }

        public void InsertAppointmentSchedule(AppointmentSchedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            _scheduleRepository.Insert(schedule);

        }

        public void UpdateAppointmentSchedule(AppointmentSchedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            _scheduleRepository.Update(schedule);
        }
        #endregion

        #region Methods(Schedule)
        public void DeleteSchedule(Schedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            _docScheduleRepository.Delete(schedule);
        }

        public IList<Schedule> GetAllSchedule()
        {
            var query = _docScheduleRepository.Table;
            return query.ToList();
        }

        public Schedule GetScheduleById(int scheduleId)
        {
            if (scheduleId == 0)
                return null;

            return _docScheduleRepository.GetById(scheduleId);
        }

        public IList<Schedule> GetScheduleByDoctorId(string doctorId)
        {
            if (string.IsNullOrWhiteSpace(doctorId))
                return null;

            var query = from c in _docScheduleRepository.Table
                        where c.DoctorId.Equals(doctorId)
                        select c;

            return query.ToList();
        }

        public IList<Schedule> GetScheduleByIds(int[] scheduleIds)
        {
            if (scheduleIds == null || scheduleIds.Length == 0)
                return new List<Schedule>();

            var query = from c in _docScheduleRepository.Table
                        where scheduleIds.Contains(c.Id)
                        select c;
            var schedules = query.ToList();
            return scheduleIds.Select(id => schedules.Find(x => x.Id == id)).Where(schedule => schedule != null).ToList();
        }

        public void InsertSchedule(Schedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            _docScheduleRepository.Insert(schedule);

        }

        public void UpdateSchedule(Schedule schedule)
        {
            if (schedule == null)
                throw new ArgumentNullException(nameof(schedule));

            _docScheduleRepository.Update(schedule);
        }

        #endregion

        #region Methods(Booking)
        public IList<AppointmentStatus> GetAllAppointmentStatus()
        {
            var query = _statusRepository.Table;
            return query.ToList();
        }

        public AppointmentStatus GetAppointmentStatusByName(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return null;

            var query = _statusRepository.Table.ToList();
            var appointmentStatuses = query.Where(s => s.Name.Trim().ToLower() == status.Trim().ToLower());

            return appointmentStatuses.FirstOrDefault();
        }

        #endregion
    }
}
