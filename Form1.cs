using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DxLibDLL;//DxLibを使用

namespace DesktopMascot
{
    public partial class Form1 : Form
    {
        private int modelHandle;
        private int attachIndex;
        private float totalTime;
        private float playTime;
        private float playSpeed;

        public Form1()
        {
            InitializeComponent();//フォームの初期設定

            ClientSize = new Size(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);//画面サイズの設定
            Text = "DesktopMascot";//ウインドウの名前を設定

            DX.SetOutApplicationLogValidFlag(DX.FALSE);//Log.txtを生成しないように設定
            DX.SetUserWindow(Handle);//DxLibの親ウインドウをこのフォームに設定
            DX.SetOutApplicationLogValidFlag(DX.FALSE);//Log.txtを生成しないように設定
            DX.SetUserWindow(Handle);//DxLibの親ウインドウをこのフォームに設定
            DX.SetZBufferBitDepth(24);//Zバッファの深度を24bitに変更
            DX.SetCreateDrawValidGraphZBufferBitDepth(24);//裏画面のZバッファの深度を24bitに変更
            DX.SetFullSceneAntiAliasingMode(4, 2);//画面のフルスクリーンアンチエイリアスモードの設定をする
            DX.DxLib_Init();//DxLibの初期化処理
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);//描画先を裏画面に設定
            DX.DxLib_Init();//DxLibの初期化処理
            DX.SetDrawScreen(DX.DX_SCREEN_BACK);//描画先を裏画面に設定

            modelHandle = DX.MV1LoadModel("Data/結月ゆかり_純.pmd");//3Dモデルの読み込み
            attachIndex = DX.MV1AttachAnim(modelHandle, 1, -1, DX.FALSE);//モーションの選択  引数2番目がロードするモーションの末尾番号
            totalTime = DX.MV1GetAttachAnimTotalTime(modelHandle, attachIndex);//モーションの総再生時間を取得
            playTime = 0.0f;//モーションの再生位置
            playSpeed = 0.4f;//モーションの再生位置を進める速度

            DX.SetCameraNearFar(0.1f, 1000.0f);//奥行0.1～1000をカメラの描画範囲とする
            DX.SetCameraPositionAndTarget_UpVecY(DX.VGet(12.0f, 25.0f, -35.0f), DX.VGet(0.0f, 15.0f, 0.0f));//第1引数の位置から第2引数の位置を見る角度にカメラを設置
        }

        public void MainLoop()
        {
            DX.ClearDrawScreen();//裏画面を消す
            DX.DrawBox(0, 0, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, DX.GetColor(1, 1, 1), DX.TRUE);//背景を設定(透過させる)

            playTime += playSpeed;//時間を進める

            //モーションの再生位置が終端まで来たら最初に戻す
            if (playTime >= totalTime)
            {
                playTime = 0.0f;
            }

            DX.MV1SetAttachAnimTime(modelHandle, attachIndex, playTime);//モーションの再生位置を設定


            DX.MV1DrawModel(modelHandle);//3Dモデルの描画

            //ESCキーを押したら終了
            if (DX.CheckHitKey(DX.KEY_INPUT_ESCAPE) != 0)
            {
                Close();
            }

            DX.ScreenFlip();//裏画面を表画面にコピー
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            DX.DxLib_End();//DxLibの終了処理
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            FormBorderStyle = FormBorderStyle.None;//フォームの枠を非表示にする
            TransparencyKey = Color.FromArgb(1, 1, 1);//透過色を設定
        }

    }
}