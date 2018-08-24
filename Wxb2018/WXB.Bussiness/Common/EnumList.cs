using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WXB.Bussiness.Common
{

    #region 登录返回状态码
    /// <summary>
    /// 登录返回状态码
    /// </summary>
    public enum EnumLoginCode
    {

        /// <summary>
        //  登录成功
        /// </summary>
        登录成功 = 200,

        /// <summary>
        /// 用户名或密码为空
        /// </summary>
        用户名或密码为空 = -400,

        /// <summary>
        /// 用户不存在
        /// </summary>
        用户不存在 = -404
    }
    #endregion

    #region 操作类型
    /// <summary>
    /// 操作类型
    /// </summary>
    public enum EnumOperateType
    {
        /// <summary>
        /// 登录
        /// </summary>
        登录 = 1,

        /// <summary>
        /// 退出
        /// </summary>
        退出 = 5,

        /// <summary>
        /// 添加通知
        /// </summary>
        添加 = 10,

        /// <summary>
        /// 编辑通知
        /// </summary>
        编辑 = 15,

        /// <summary>
        /// 删除通知
        /// </summary>
        删除 = 20,

        /// <summary>
        /// 上班
        /// </summary>
        上班 = 25,

        /// <summary>
        /// 下班
        /// </summary>
        下班 = 30,

        /// <summary>
        /// 编辑反馈
        /// </summary>
        编辑反馈 = 35
    }
    #endregion

    #region 角色类型
    /// <summary>
    /// 角色类型
    /// </summary>
    public enum EnumRoleType
    {
        /// <summary>
        /// 总编
        /// </summary>
        总编 = 1,

        /// <summary>
        /// 责编
        /// </summary>
        责编 = 2,

        /// <summary>
        /// 编辑
        /// </summary>
        编辑 = 3,

        /// <summary>
        /// 管理员
        /// </summary>
        管理员 = 4,

        /// <summary>
        /// 总监
        /// </summary>
        总监 = 5,

        /// <summary>
        /// 分类员
        /// </summary>
        分类员 = 6

    }
    #endregion

    #region 指定人员方式
    /// <summary>
    /// 指定人员方式
    /// </summary>
    public enum EnumChooseUserWay
    {

        /// <summary>
        /// 角色
        /// </summary>
        角色 = 1,

        /// <summary>
        /// 个人
        /// </summary>
        个人 = 5
    }
    #endregion

    #region 应急状态
    /// <summary>
    /// 应急状态
    /// </summary>
    public enum EnumResponseStatus
    {
        /// <summary>
        /// 
        /// </summary>
        应急 = 1,

        /// <summary>
        /// 
        /// </summary>
        加强应急 = 5
    }
    #endregion

    #region 值班状态
    /// <summary>
    /// 值班状态
    /// </summary>
    public enum EnumDutyStatus
    {
        /// <summary>
        /// 上班
        /// </summary>
        上班 = 1,

        /// <summary>
        /// 下班
        /// </summary>
        下班 = 5
    }
    #endregion

    /// <summary>
    /// 是否是管理员
    /// </summary>
    public enum EnumIsAdmin
    {
        /// <summary>
        /// 是
        /// </summary>
        是 = 0,

        /// <summary>
        /// 否
        /// </summary>
        否 = 1
    }

}
