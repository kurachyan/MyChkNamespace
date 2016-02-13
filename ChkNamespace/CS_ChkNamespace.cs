using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LRSkip;
using Rsvwrd;

namespace ChkNamespace
{
    public class CS_ChkNamespace
    {
        #region 共有領域
        CS_LRskip lrskip;           // 両側余白情報を削除
        CS_Rsvwrd rsvwrd;           // 予約語を確認する

        private String _wbuf;
        private Boolean _empty;
        public String Wbuf
        {
            get
            {
                return (_wbuf);
            }
            set
            {
                _wbuf = value;
                if (_wbuf == null)
                {   // 設定情報は無し？
                    _empty = true;
                }
                else
                {   // 整形処理を行う
                    if (lrskip == null)
                    {   // 未定義？
                        lrskip = new CS_LRskip();
                    }
                    lrskip.Exec(_wbuf);
                    _wbuf = lrskip.Wbuf;

                    // 作業の為の下処理
                    if (_wbuf.Length == 0 || _wbuf == null)
                    {   // バッファー情報無し
                        // _wbuf = null;
                        _empty = true;
                    }
                    else
                    {
                        _empty = false;
                    }
                }
            }
        }
        private String _result;     // [Namespace]ＬＢＬ情報
        public String Result
        {
            get
            {
                return (_result);
            }
            set
            {
                _result = value;
            }
        }

        // 予約語：「ＮａｍｅＳｐａｃｅ」
        const int RSV_NONE = 0;     // 未定義
        const int RSV_OTHER = 6;    // 予約語６：その他
//        const string RSV_KEYWORD = "namespace";
//        private int _rsvcode;

/*
        public Boolean Rsv
        {   // 予約語有無確認　[false]:予約語なし [true]:予約語あり
            get
            {
                if (_rsvcode == RSV_NONE)
                {   // 予約語未検出？
                    return (false);
                }
                else
                {   // 予約語検出
                    if (_rsvcode == RSV_OTHER)
                    {   // 予約語６検出？
                        return (true);
                    }
                    else
                    {   // 予約語６以外検出
                        return (false);
                    }
                }
            }
        }
        public int RsvKind
        {
            get
            {
                return ((int)_rsvcode);
            }
        }
*/

        private static Boolean _Is_namespace;
        public Boolean Is_namespace
        {
            get
            {
                return (_Is_namespace);
            }
            set
            {
                _Is_namespace = value;
            }
        }
#endregion

#region コンストラクタ
        public CS_ChkNamespace()
        {   // コンストラクタ
            _wbuf = null;       // 設定情報無し
            _empty = true;

//            _rsvcode = RSV_NONE;    // 予約語：未定義
            _Is_namespace = false;  // [namespace]フラグ：false

            rsvwrd = new CS_Rsvwrd();           // 予約語を確認する
        }
#endregion

#region モジュール
        public void Clear()
        {   // 作業領域の初期化
            _wbuf = null;       // 設定情報無し
            _empty = true;

//            _rsvcode = RSV_NONE;    // 予約語：未定義
            _Is_namespace = false;  // [namespace]フラグ：false
        }

        public void Exec()
        {   // "namespace"評価
            if (!_empty)
            {   // バッファーに実装有り
                rsvwrd.Exec(_wbuf);     // 評価情報の予約語確認を行う

                if (_Is_namespace)
                {   // [namespace]フラグは、true？
                    if (!rsvwrd.Is_namespace)
                    {   // 評価情報は、非予約語？
                        // ＬＢＬ情報テーブルに、namespace名を登録する
                        _Is_namespace = false;       // [namespace]フラグ：false
                    }
                }
                else
                {   // [namespace]フラグは、false
                    if (rsvwrd.Is_namespace)
                    {   // 評価情報は、"namespace"？
                        _Is_namespace = true;       // [namespace]フラグ：true
                        rsvwrd.Is_namespace = false;
                    }
                }
            }
        }
#endregion

    }
}