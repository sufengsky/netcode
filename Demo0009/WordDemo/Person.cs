using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WordDemo
{
    public class Person
    {
        /// <summary>
        /// 序号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        public string Birthday { get; set; }
        /// <summary>
        /// 公民身份证号码
        /// </summary>
        public string CardNo { get; set; }
        /// <summary>
        /// 住址状态
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 户主
        /// </summary>
        public string HostName { get; set; }
        /// <summary>
        /// 与户主关系
        /// </summary>
        public string Relation { get; set; }
        /// <summary>
        /// 持股份额
        /// </summary>
        public string Percent { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public string total { get; set; } = "";

        public string MobilePhone { get; set; }
    }
}
