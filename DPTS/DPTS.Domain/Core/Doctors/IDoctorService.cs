using System;
using System.Collections.Generic;
using DPTS.Domain.Entities;

namespace DPTS.Domain.Core.Doctors
{
    public interface IDoctorService
    {
        /// <summary>
        /// Inserts an Doctor account
        /// </summary>
        void AddDoctor(Doctor doctor);

        /// <summary>
        /// Get Doctor by id
        /// </summary>
        Doctor GetDoctorbyId(string doctorId);

        /// <summary>
        /// Delete Doctor by id
        /// </summary>
        void DeleteDoctor(Doctor Doctor);

        /// <summary>
        /// update catDoctoregory
        /// </summary>
        void UpdateDoctor(Doctor data);

        /// <summary>
        /// get list of Doctor Name
        /// </summary>
        IList<string> GetDoctorsName(bool showhidden);

        /// <summary>
        /// search doctor
        /// </summary>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="totalCount"></param>
        /// <param name="zipcode"></param>
        /// <returns></returns>
        IList<Doctor> SearchDoctor(int page, int itemsPerPage, out int totalCount, string zipcode = null, int specialityId = 0, string searchByName = null);//, double Geo_Distance = 50);

        /// <summary>
        /// search doc price range
        /// </summary>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="totalCount"></param>
        /// <param name="zipcode"></param>
        /// <param name="specialityId"></param>
        /// <param name="searchByName"></param>
        /// <param name="maxFee"></param>
        /// <param name="minFee"></param>
        /// <returns></returns>
        IList<Doctor> SearchDoctor(int page, int itemsPerPage, out int totalCount, string zipcode = null, int specialityId = 0, string searchByName = null, decimal maxFee = 0, decimal minFee = 0);

        /// <summary>
        /// Paging with get all doctors
        /// </summary>
        /// <param name="page"></param>
        /// <param name="itemsPerPage"></param>
        /// <param name="totalCount"></param>
        /// <returns></returns>
        IList<Doctor> GetAllDoctors(int page, int itemsPerPage, out int totalCount);

        #region Social links

        void InsertSocialLink(SocialLinkInformation link);

        SocialLinkInformation GetSocialLinkbyId(int id);

        void DeleteSocialLink(SocialLinkInformation link);

        void UpdateSocialLink(SocialLinkInformation link);

        IPagedList<SocialLinkInformation> GetAllLinksByDoctor(string doctorId,int pageIndex = 0,
            int pageSize = Int32.MaxValue, bool showHidden = false);

        #endregion

        #region HonorsAwards
        void InsertHonorsAwards(HonorsAwards award);

        HonorsAwards GetHonorsAwardsbyId(int id);

        void DeleteHonorsAwards(HonorsAwards award);

        void UpdateHonorsAwards(HonorsAwards award);

        IPagedList<HonorsAwards> GetAllHonorsAwards(string doctorId, int pageIndex = 0,
            int pageSize = Int32.MaxValue, bool showHidden = false);
        #endregion

        #region Education
        void InsertEducation(Education education);

        Education GetEducationbyId(int id);

        void DeleteEducation(Education education);

        void UpdateEducation(Education education);

        IPagedList<Education> GetAllEducation(string doctorId, int pageIndex = 0,
            int pageSize = Int32.MaxValue, bool showHidden = false);
        #endregion

        #region Experience
        void InsertExperience(Experience experience);

        Experience GetExperiencebyId(int id);

        void DeleteExperience(Experience experience);

        void UpdateExperience(Experience experience);

        IPagedList<Experience> GetAllExperience(string doctorId, int pageIndex = 0,
            int pageSize = Int32.MaxValue, bool showHidden = false);
        #endregion

        #region Picture
        void InsertDoctorPicture(PictureMapping docPicture);

        IList<PictureMapping> GetDoctorPicturesByDoctorId(string doctorId);

        PictureMapping GetDoctorPictureById(int doctorPictureId);

        void UpdateDoctorPicture(PictureMapping docPicture);

        void DeleteDoctorPicture(PictureMapping docPicture);
        #endregion

        //#region Doctor reviews

        //IPagedList<DoctorReview> GetAllProductReviews(string visitorId, bool? approved,
        //    DateTime? fromUtc = null, DateTime? toUtc = null,
        //    string message = null, int storeId = 0, string doctorId = null,
        //    int pageIndex = 0, int pageSize = int.MaxValue);

        //DoctorReview GetDoctorReviewById(int doctorReviewId);

        //IList<DoctorReview> GetDoctorReviewsByIds(int[] doctorReviewIds);

        ///// <summary>
        ///// Deletes a doctor review
        ///// </summary>
        ///// <param name="productReview">Product review</param>
        //void DeleteDoctorReview(DoctorReview productReview);

        ///// <summary>
        ///// Deletes doctor reviews
        ///// </summary>
        ///// <param name="productReviews">Product reviews</param>
        //void DeleteDoctorReviews(IList<DoctorReview> doctorReviews);

        //#endregion
    }
}
