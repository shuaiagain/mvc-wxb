using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

using WXB.Bussiness.ViewModels;
using WXB.Bussiness.Service;
using WXB.Bussiness.Models;
using WXB.Bussiness.Common;
using System.IO;
using System.Text;
using Wxb2018.Filters;

namespace Wxb2018.Controllers
{
    public class HomeController : BaseController
    {

        #region Index
        /// <summary>
        /// Index
        /// </summary>
        /// <returns></returns>
        [BaseFilter]
        public ActionResult Index()
        {
            try
            {
                PageVM<NoticeVM> data = new NoticeService().GetNoticeListPage(new NoticeQuery() { PageSize = 8 });

                ViewBag.UserData = this.UserData;
                return View(data);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 获取通知列表(分部视图分页)
        /// <summary>
        /// GetNoticePageData
        /// </summary>
        /// <returns></returns>
        public ActionResult GetNoticePageData(NoticeQuery query)
        {

            if (query == null) query = new NoticeQuery();

            if (!query.PageIndex.HasValue)
                query.PageIndex = 1;
            if (!query.PageSize.HasValue)
                query.PageSize = 8;

            try
            {
                PageVM<NoticeVM> list = new NoticeService().GetNoticeListPage(query);
                if (list == null || list.Data == null || list.Data.Count == 0)
                {
                    return Json(new
                    {
                        Code = -200,
                        Msg = "暂无数据"
                    });
                }

                return Json(new
                {
                    Code = 200,
                    Msg = "获取成功",
                    Data = list
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 添加通知
        /// <summary>
        /// 添加通知
        /// </summary>
        /// <returns></returns>
        [RoleRightsFilter]
        public ActionResult Add()
        {
            return View();
        }
        #endregion

        #region 编辑
        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        [RoleRightsFilter]
        public ActionResult Edit(int noticeId)
        {
            NoticeVM notice = new NoticeService().GetNoticeById(noticeId);

            return View("add", notice);
        }
        #endregion

        #region 保存通知

        /// <summary>
        /// 保存通知
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveNotice(NoticeVM vm)
        {
            ResultEntity<NoticeVM> result = new ResultEntity<NoticeVM>();
            if (vm == null || string.IsNullOrEmpty(vm.Title) || string.IsNullOrEmpty(vm.Content))
            {
                result.Code = -400;
                result.Msg = "通知标题和通知内容不能为空";
                return Json(result);
            }

            try
            {
                if (!vm.ID.HasValue)
                {
                    vm.Inputer = UserData.Name;
                    vm.InputerID = UserData.UserId;
                    vm.InputTime = DateTime.Now;
                }

                vm.UpdateTime = DateTime.Now;

                #region 日志

                var log = new LogVM()
                        {
                            Operator = this.UserData.Name,
                            OperatorID = this.UserData.UserId,
                            RoleTypes = this.UserData.RoleTypes,
                            OperateTime = DateTime.Now,
                            OperateType = vm.ID.HasValue ? (int)EnumOperateType.编辑 : (int)EnumOperateType.添加
                        };

                log.OperateDescribe = ((EnumOperateType)log.OperateType).ToString() + "通知：Title = " + vm.Title;
                Logger.AddLog(log);

                #endregion

                vm = new NoticeService().Save(vm);
                result.Data = vm;

            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }

            result.Msg = "添加成功";
            result.Code = 200;

            return Json(result);
        }

        #endregion

        #region 删除通知
        /// <summary>
        /// 删除通知
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public ActionResult Delete(int noticeId)
        {
            try
            {
                bool result = new NoticeService().Delete(noticeId);
                if (!result) return Json(new { Code = -200, Msg = "删除失败" });

                #region 日志

                var log = new LogVM()
                    {
                        Operator = this.UserData.Name,
                        OperatorID = this.UserData.UserId,
                        RoleTypes = this.UserData.RoleTypes,
                        OperateTime = DateTime.Now,
                        OperateType = (int)EnumOperateType.删除
                    };

                log.OperateDescribe = ((EnumOperateType)log.OperateType).ToString() + "通知：ID = " + noticeId;
                Logger.AddLog(log);

                #endregion

                return Json(new { Code = 200, Msg = "删除成功" });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 通知预览
        /// <summary>
        /// 通知预览
        /// </summary>
        /// <param name="noticeId"></param>
        /// <param name="noticeReceiverId"></param>
        /// <returns></returns>
        [BaseFilter]
        public ActionResult ViewNotice(int noticeId, int noticeReceiverId = 0)
        {
            try
            {
                UserItemVM user = new UserItemVM()
                   {
                       ID = UserData.UserId.Value,
                       Name = UserData.Name
                   };

                NoticeVM notice = new NoticeService().GetNotice(noticeId, user, noticeReceiverId);
                return View(notice);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 获取用户列表
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        public ActionResult GetUserList()
        {
            try
            {
                List<UserItemVM> userList = new UserService().GetUserList();
                if (userList == null) return Json(new { Code = -200, Msg = "暂无数据" });

                return Json(new { Code = 200, Msg = "获取成功", Data = userList }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }

        #endregion

        #region 上传文件
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public ActionResult UploadFile(HttpPostedFileBase file)
        {
            try
            {
                string filePathName = string.Empty;

                string localPath = Path.Combine(HttpRuntime.AppDomainAppPath, "Resource/UploadFiles");
                localPath = Path.Combine(Server.MapPath("~/Resource/UploadFiles"));
                if (Request.Files.Count == 0)
                {
                    return Json(new { Code = -404, Msg = "没有上传文件" }, JsonRequestBehavior.AllowGet);
                }

                string ex = Path.GetExtension(file.FileName);
                //filePathName = Guid.NewGuid().ToString("N") + ex;
                filePathName = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss-fff") + ex;
                if (!System.IO.Directory.Exists(localPath))
                {
                    System.IO.Directory.CreateDirectory(localPath);
                }

                file.SaveAs(Path.Combine(localPath, filePathName));

                return Json(new
                {
                    Code = 200,
                    Msg = "保存成功",
                    Data = new
                    {
                        FilePath = Path.Combine(localPath, filePathName),
                        ViewUrl = "~/Resource/UploadFiles/" + filePathName,
                        FilePathName = filePathName
                    }
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 下载文件
        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Download(string filePath, string fileName)
        {
            try
            {
                Encoding encoding;
                string outputFileName = null;
                fileName = fileName.Replace("'", "");

                //string browser = Request.UserAgent.ToUpper();
                //if (browser.Contains("MS") == true && browser.Contains("IE") == true)
                //{
                //    outputFileName = HttpUtility.UrlEncode(fileName);
                //    encoding = Encoding.Default;
                //}
                //else if (browser.Contains("FIREFOX") == true)
                //{
                //    outputFileName = fileName;
                //    encoding = Encoding.GetEncoding("GB2312");
                //}
                //else
                //{
                outputFileName = HttpUtility.UrlEncode(fileName, Encoding.UTF8);
                encoding = Encoding.UTF8;
                //}

                FileStream fs = new FileStream(Server.MapPath(filePath), FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                byte[] bytes = new byte[(int)fs.Length];
                fs.Read(bytes, 0, bytes.Length);
                fs.Close();
                Response.Charset = "UTF-8";
                Response.ContentType = "application/octet-stream";
                Response.ContentEncoding = encoding;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + outputFileName);
                Response.BinaryWrite(bytes);
                Response.Flush();
                Response.End();
                return new EmptyResult();
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

        #region 添加/编辑反馈
        /// <summary>
        ///  添加/编辑反馈
        /// </summary>
        /// <param name="noticeId"></param>
        /// <returns></returns>
        public ActionResult EditFeedback(int noticeId)
        {
            NoticeVM notice = new NoticeService().GetNoticeById(noticeId);

            return View("EditFeedback", notice);
        }
        #endregion

        #region 获取通知附件
        public ActionResult ShowAttachment(int noticeId)
        {
            NoticeVM notice = new NoticeService().GetNoticeById(noticeId);

            return View("ShowAttachment", notice);
        }
        #endregion

        #region 保存反馈
        /// <summary>
        /// 保存反馈
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult SaveFeedback(NoticeVM vm)
        {
            if (vm == null || !vm.ID.HasValue)
            {
                return Json(new
                {
                    Code = -400,
                    Msg = "参数不能为空"
                });
            }

            try
            {
                vm.FeedBacker = UserData.Name;
                vm.FeedBackerID = UserData.UserId;
                new NoticeService().SaveFeedback(vm);

                #region 日志
                var log = new LogVM()
                {
                    Operator = this.UserData.Name,
                    OperatorID = this.UserData.UserId,
                    RoleTypes = this.UserData.RoleTypes,
                    OperateTime = DateTime.Now,
                    OperateType = (int)EnumOperateType.编辑反馈
                };

                log.OperateDescribe = ((EnumOperateType)log.OperateType).ToString() + ",通知：Title = " + vm.Title;
                Logger.AddLog(log);
                #endregion

                return Json(new
                {
                    Code = 200,
                    Msg = "保存成功",
                });
            }
            catch (Exception ex)
            {
                LogHelper.WriteLog(ex.Message);
                throw ex;
            }
        }
        #endregion

    }
}