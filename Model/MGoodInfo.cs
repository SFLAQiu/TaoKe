using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LG.Utility;


namespace TaoKeModel {
    public class MGoodInfo {
        /// <summary>
        /// 商品ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 现价
        /// </summary>
        public decimal NowPrice { get; set; }
        /// <summary>
        /// 原价格
        /// </summary>
        public decimal OldPrice { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public EGoodType Type { get; set; }
        /// <summary>
        /// 来源商城
        /// </summary>
        public ESourceMall SourceMall { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDateTime { get; set; }
        /// <summary>
        /// 淘宝商品ID
        /// </summary>
        public int GoodId { get; set; }
        /// <summary>
        /// 商品图片地址
        /// </summary>
        public string ImgUrl { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal Discount{
            get{
                var discount = 0m;
                if (this.NowPrice < this.OldPrice) discount = decimal.Round(this.NowPrice / (this.OldPrice * 1.0m), 2)*10;
                return discount;
            }
        }
        /// <summary>
        /// 购买地址
        /// </summary>
        public string BuyUrl {
            get;
            set;
        }
                

    }

    public class MGoodInfoSearch {
        /// <summary>
        /// 商品名称
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public EGoodType? Type { get; set; }
        /// <summary>
        /// 来源商城
        /// </summary>
        public ESourceMall? SourceMall { get; set; }
    }

    public enum ESourceMall {
        /// <summary>
        /// 淘宝
        /// </summary>
        [EnumAttr(Text="淘宝")]
        TaoBao = 1,
        /// <summary>
        /// 天猫
        /// </summary>
        [EnumAttr(Text = "天猫")]
        Tmall = 2
    }
    public enum EGoodType {
        /// <summary>
        /// 婴幼穿着
        /// </summary>
         [EnumAttr(Text = "婴幼穿着")]
        Clothing = 1,
        /// <summary>
         /// 纸尿裤
        /// </summary>
         [EnumAttr(Text = "纸尿裤")]
         BabyDiaper = 2,
         /// <summary>
         /// 婴幼用品
         /// </summary>
         [EnumAttr(Text = "婴幼用品")]
         articles = 3,
         /// <summary>
         /// 玩具早教
         /// </summary>
         [EnumAttr(Text = "玩具早教")]
         toy = 4,
         /// <summary>
         /// 玩具早教
         /// </summary>
         [EnumAttr(Text = "孕妈必备")]
         mama =5,
    }
}
