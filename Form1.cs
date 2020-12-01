﻿using System;
using System.Windows.Forms;
using OpenTK.Input;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace ClothForm
{
    public partial class Form1 : Form
    {
        private static int mouseX;
        private static int mouseY;
        private static bool mouseDownLeft;
        private static bool runSim;
        private static bool[] keys = new bool[256];
        private static bool mesh;
        private static Stopwatch timer = new Stopwatch();
        private Cloth cloth;
        private int textureID;
        private float curAngleHorizontal;
        private float curAngleVertical;
        private float lastTime;
        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            runSim = false;
            mouseDownLeft = false;
           // timer1.Interval = 1000 / 60;
          //  timer1.Enabled = true;
            mesh = false;
           // cloth = new Cloth();
           // cloth.AddCollider(Vector3.Zero, 5); 
          //  lastTime = timer.ElapsedMilliseconds / 1000.0f;//Перенести
         //   timer.Start();
            curAngleHorizontal = 0;
            curAngleVertical = 0;
            Bitmap image = new Bitmap(Image.FromFile("cloth.png"));
            textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            image.UnlockBits(data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
        }
        
        [STAThread]
        private void timer1_Tick(object sender, EventArgs e)
        {
            renderFrame();
            updateFrame();
            glControl1.SwapBuffers();
        }

        private void renderFrame()
        {
            #region FRAME SETUP
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Greater, 0.5f);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            #endregion
            #region LIGHTING
            float[] mat_specular = { 1.0f, 1.0f, 1.0f, 1.0f };
            float[] mat_shininess = { 50.0f };
            float[] light_position = { 1000.0f, 500.0f, 1000.0f, 100.0f };
            float[] light_ambient = { 0.5f, 0.5f, 0.5f, 1.0f };
            GL.Light(LightName.Light0, LightParameter.Position, light_position);
            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.Light0);
            GL.Enable(EnableCap.ColorMaterial);
            #endregion
            #region TEXTURING
            GL.Enable(EnableCap.Texture2D);
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
            #endregion
            #region PROJECTION MATRIX
            float fov = MathHelper.DegreesToRadians(60);
            float aspect = glControl1.Width / (float)glControl1.Height;
            float zNear = 0.1f;
            float zFar = 500;
            var projMat = Matrix4.CreatePerspectiveFieldOfView(fov, aspect, zNear, zFar);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projMat);
            #endregion
            #region CAMERA MATRIX
            GL.MatrixMode(MatrixMode.Modelview);
            var camPos = new Vector3(0, 15, 25);
            var lookMat = Matrix4.LookAt(camPos, Vector3.Zero, Vector3.UnitY);
            var modelMat = Matrix4.CreateRotationY(curAngleHorizontal);
            lookMat = modelMat * lookMat;
           // modelMat = Matrix4.CreateRotationZ(curAngleVertical);
           // lookMat = modelMat * lookMat;
            GL.LoadMatrix(ref lookMat);
            #endregion
            #region RENDERING
            if (mesh)
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }
            GL.Begin(PrimitiveType.Triangles);
            foreach (var t in cloth.triangles)
            {
                var A = cloth.vertices[t.A];
                var B = cloth.vertices[t.B];
                var C = cloth.vertices[t.C];

                if (!mesh) GL.Color4(Color.White);
                else GL.Color4(Color.Red);
                GL.Normal3(A.normal.X, A.normal.Y, A.normal.Z);
                if (!mesh) GL.TexCoord2(A.uv.X, A.uv.Y);
                GL.Vertex3(A.position.X, A.position.Y, A.position.Z);
                GL.Normal3(B.normal.X, B.normal.Y, B.normal.Z);
                if (!mesh) GL.TexCoord2(B.uv.X, B.uv.Y);
                GL.Vertex3(B.position.X, B.position.Y, B.position.Z);
                GL.Normal3(C.normal.X, C.normal.Y, C.normal.Z);
                if (!mesh) GL.TexCoord2(C.uv.X, C.uv.Y);
                GL.Vertex3(C.position.X, C.position.Y, C.position.Z);
            }
            GL.End();
            #endregion
        }

        private void updateFrame()
        {
            if (runSim) {
                float curTime = timer.ElapsedMilliseconds / 1000.0f;
                float delta = curTime - lastTime;
                lastTime = curTime;
                cloth.Simulate(delta);
            }
            if (mouseDownLeft) {
                curAngleHorizontal = (mouseX / (float)glControl1.Width) * MathHelper.DegreesToRadians(360);
                curAngleVertical = (mouseY / (float)glControl1.Height) * MathHelper.DegreesToRadians(360);
            }
        }

        private void glControl1_KeyDown(object sender, KeyEventArgs e)
        {
            keys[(int)e.KeyCode] = true;
        }

        private void glControl1_Click(object sender, EventArgs e)
        {
            
        }

        private void glControl1_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownLeft = true;
            }
        }

        private void пункт2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void glControl1_KeyUp(object sender, KeyEventArgs e)
        {
            keys[(int)e.KeyCode] = false;
        }

        private void glControl1_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
        }

        private void glControl1_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                mouseDownLeft = false;
            }
            
        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
        }

        private void перезапуститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cloth.Reset();
        }

        private void начатьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            runSim = !runSim;
            if(runSim) {
                lastTime = timer.ElapsedMilliseconds / 1000.0f;//Перенести
                timer.Start();
            } else {
                timer.Stop();
            }
        }

        private void создатьТканьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Interval = 1000 / 60;
            timer1.Enabled = true;
            cloth = new Cloth();
            cloth.AddCollider(Vector3.Zero, 5);
        }

        private void отчиститьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //timer1.Stop();
        }
    }
}
