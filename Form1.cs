using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using OpenTK;
using OpenTK.Graphics;
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
        private static Stopwatch timer = new Stopwatch();
        private Cloth cloth;
        private int textureID;
        private float curAngleHorizontal;
        private float curAngleVertical;
        private float lastTime;
        private float xSphere, ySphere, zSphere, rSphere;
        private static bool dMesh, dPoints, dSprings, dSphere;
        public Form1()
        {
            InitializeComponent();
        }

        private void glControl1_Load(object sender, EventArgs e)
        {
            runSim = false;
            mouseDownLeft = false;
            curAngleHorizontal = 0;
            curAngleVertical = 0;
            xSphere = 0;
            ySphere = 0;
            zSphere = 0;
            rSphere = 6;
            Bitmap image = new Bitmap(Image.FromFile("cloth.png"));
            textureID = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, textureID);
            BitmapData data = image.LockBits(new System.Drawing.Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, data.Width, data.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            image.UnlockBits(data);
            GL.GenerateMipmap(GenerateMipmapTarget.Texture2D);
            dMesh = false;
            dPoints = false;
            dSprings = false;
            dSphere = false;
            timer1.Interval = 1000 / 60;
            timer1.Enabled = true;
            showToolStripMenuItem.Enabled = false;
            simulationToolStripMenuItem.Enabled = false;
            createSphereToolStripMenuItem.Enabled = false;
            clearToolStripMenuItem.Enabled = false;
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
            GL.ClearColor(0.2f, 0.2f, 0.4f, 0.5f);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Greater, 0.5f);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Lequal);
            #endregion
            #region LIGHTING
            float[] light_position = {1000.0f, 500.0f, 1000.0f, 100.0f};
            float[] light_ambient = {0.5f, 0.5f, 0.5f, 1.0f};
            float[] light_diffuze = {0.5f, 0.5f, 0.3f, 0.0f};
            GL.Light(LightName.Light0, LightParameter.Position, light_position);
            GL.Light(LightName.Light0, LightParameter.Ambient, light_ambient);
            GL.Light(LightName.Light0, LightParameter.Diffuse, light_diffuze);
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
            modelMat = Matrix4.CreateRotationZ(curAngleVertical);
            lookMat = modelMat * lookMat;
            GL.LoadMatrix(ref lookMat);
            #endregion
            #region RENDERING
            if (dMesh) {
                GL.Begin(PrimitiveType.Triangles);
                foreach (var t in cloth.getTriangles) {
                    var A = cloth.getVertices[t.A];
                    var B = cloth.getVertices[t.B];
                    var C = cloth.getVertices[t.C];
                    GL.Color4(Color.White);
                    GL.Normal3(A.normal.X, A.normal.Y, A.normal.Z);
                    GL.TexCoord2(A.uv.X, A.uv.Y);
                    GL.Vertex3(A.position.X, A.position.Y, A.position.Z);
                    GL.Normal3(B.normal.X, B.normal.Y, B.normal.Z);
                    GL.TexCoord2(B.uv.X, B.uv.Y);
                    GL.Vertex3(B.position.X, B.position.Y, B.position.Z);
                    GL.Normal3(C.normal.X, C.normal.Y, C.normal.Z);
                    GL.TexCoord2(C.uv.X, C.uv.Y);
                    GL.Vertex3(C.position.X, C.position.Y, C.position.Z);
                }
                GL.End();
            }
            if (dSphere) {
                GL.Color4(Color.White);
                GL.Enable(EnableCap.Lighting);
                GL.Material(MaterialFace.Front, MaterialParameter.Ambient, new Color4(1.0f, 0.0f, 0.0f, 0.0f));
                GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, new Color4(1.0f, 0.0f, 0.0f, 0.0f));
                GL.Material(MaterialFace.Front, MaterialParameter.Specular, Color.White);
                GL.Material(MaterialFace.Front, MaterialParameter.Shininess, 32.0f);
                GL.Enable(EnableCap.CullFace);
                drawSpehere(rSphere - 0.5, 100, 100, xSphere, ySphere, zSphere);
                cloth.changePosistionSphere(new Vector3(xSphere, ySphere, zSphere));
                GL.Disable(EnableCap.CullFace);
                GL.Disable(EnableCap.Lighting);
            }
            if (dPoints) {
                GL.Material(MaterialFace.Front, MaterialParameter.Specular, Color.Black);
                GL.PointSize(8f);
                GL.Color3(1.0f, 0.0f, 0.0f);
                GL.Begin(PrimitiveType.Points);
                foreach (var p in cloth.getVertices) {
                    GL.Vertex3(p.position);
                }
                GL.End();
            }
            if (dSprings) {
                GL.Color3(0.0f, 0.0f, 1.0f);
                GL.LineWidth(4f);
                GL.Begin(PrimitiveType.Lines);
                foreach (var p in cloth.getSprings) {
                    GL.Vertex3(cloth.getVertices[p.P1].position);
                    GL.Vertex3(cloth.getVertices[p.P2].position);
                }
                GL.End();
            }
            #endregion
        }

        private void updateFrame()
        {
            if (runSim) {
                float curTime = timer.ElapsedMilliseconds / 1000.0f;
                float delta = curTime - lastTime;
                lastTime = curTime;
                cloth.simulate(delta);
            }
            if (mouseDownLeft) {
                curAngleHorizontal = (mouseX / (float)glControl1.Width) * MathHelper.DegreesToRadians(360);
                curAngleVertical = (mouseY / (float)glControl1.Height) * MathHelper.DegreesToRadians(360);
            }
            if(keys[(int)Keys.W])
            {
                zSphere -= 0.5f;
            }
            if (keys[(int)Keys.S])
            {
                zSphere += 0.5f;
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

        private void showPointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dPoints = !dPoints;
            showPointsToolStripMenuItem.Checked = dPoints;
        }

        private void showSpringsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dSprings = !dSprings;
            showSpringsToolStripMenuItem.Checked = dSprings;
        }

        private void showMeshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dMesh = !dMesh;
            showMeshToolStripMenuItem.Checked = dMesh;
        }

        private void параметрыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void glControl1_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, glControl1.Width, glControl1.Height);
        }

        private void rebootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cloth.reset();
        }

        private void startSimToolStripMenuItem_Click(object sender, EventArgs e)
        {
            runSim = !runSim;
            if(runSim) {
                startSimToolStripMenuItem.Text = "Закончить";
                lastTime = timer.ElapsedMilliseconds / 1000.0f;//Перенести
                timer.Start();
            } else {
                startSimToolStripMenuItem.Text = "Запустить";
                timer.Stop();
            }
        }

        private void createMeshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cloth = new Cloth();
            cloth.addSphere(Vector3.Zero, rSphere);
            showToolStripMenuItem.Enabled = true;
            simulationToolStripMenuItem.Enabled = true;
            createSphereToolStripMenuItem.Enabled = true;
            clearToolStripMenuItem.Enabled = true;
            dMesh = true;
            showMeshToolStripMenuItem.Checked = dMesh;
            dSphere = true;
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void drawSpehere(double r, int nx, int ny, float cX, float cY, float cZ)
        {
            int i, ix, iy;
            double x, y, z;
            for (iy = 0; iy < ny; ++iy) {
                GL.Begin(PrimitiveType.QuadStrip);
                for (ix = 0; ix <= nx; ++ix) {
                    x = r * Math.Sin(iy * Math.PI / ny) * Math.Cos(2 * ix * Math.PI / nx) + cX;
                    y = r * Math.Sin(iy * Math.PI / ny) * Math.Sin(2 * ix * Math.PI / nx) + cY;
                    z = r * Math.Cos(iy * Math.PI / ny) + cZ;
                    GL.Normal3(x, y, z);
                    GL.Vertex3(x, y, z);
                    x = r * Math.Sin((iy + 1) * Math.PI / ny) * Math.Cos(2 * ix * Math.PI / nx) + cX;
                    y = r * Math.Sin((iy + 1) * Math.PI / ny) * Math.Sin(2 * ix * Math.PI / nx) + cY;
                    z = r * Math.Cos((iy + 1) * Math.PI / ny) + cZ;
                    GL.Normal3(x, y, z);
                    GL.Vertex3(x, y, z);
                }
                GL.End();
            }
        }
        }
}
