using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    //Mest snodd från föreläsning
    //Skar bort animationer vid hover och klick
    //Lade till att man kan klicka med högerknappen
    public class Button
    {
        public delegate void ButtonClickedDelegate(Button button);
        public event ButtonClickedDelegate OnClick = null;

        private Rectangle _myBounds;

        private Sprite _normalState;

        public bool _isFocused { get; private set; }
        private bool _isMouseDown;

        private MouseState _previousMouseState;
        private MouseState _currentMouseState;

        public Vector2 Position
        {
            get { return new Vector2(_myBounds.X, _myBounds.Y); }
            set
            {
                _myBounds.X = (int)value.X;
                _myBounds.Y = (int)value.Y;
            }
        }

        public Sprite NormalState
        {
            set
            {
                _normalState = value;
                _myBounds.Width = _normalState.Width;
                _myBounds.Height = _normalState.Height;
            }
        }

        public Button()
        {
            _myBounds = Rectangle.Empty;

            _normalState = null;

            _isFocused = false;
            _isMouseDown = false;
        }

        public void Update()
        {
            _previousMouseState = _currentMouseState;
            _currentMouseState = Mouse.GetState();

            // If the mousecursor is inside of the rectangular area of the gui button
            if (_myBounds.Contains(_currentMouseState.X, _currentMouseState.Y))
            {
                _isFocused = true;

                //Vänsterknappen
                if (_previousMouseState.LeftButton == ButtonState.Released &&
                    _currentMouseState.LeftButton == ButtonState.Pressed)
                {
                    _isMouseDown = true;
                }

                if (_previousMouseState.LeftButton == ButtonState.Pressed &&
                    _currentMouseState.LeftButton == ButtonState.Released)
                {
                    if (_isMouseDown == true)
                    {
                        // Clicked has occured
                        if (OnClick != null)
                        {
                            OnClick(this);
                        }
                    }

                    _isMouseDown = false;
                }

                //Högerknappen
                if (_previousMouseState.RightButton == ButtonState.Released &&
                    _currentMouseState.RightButton == ButtonState.Pressed)
                {
                    _isMouseDown = true;
                }

                if (_previousMouseState.RightButton == ButtonState.Pressed &&
                    _currentMouseState.RightButton == ButtonState.Released)
                {
                    if (_isMouseDown == true)
                    {
                        // Clicked has occured
                        if (OnClick != null)
                        {
                            OnClick(this);
                        }
                    }

                    _isMouseDown = false;
                }
            }
            else // If the mousecursor is outside of the rectangular area of the gui button
            {
                _isFocused = false;

                if (_previousMouseState.LeftButton == ButtonState.Pressed &&
                    _currentMouseState.LeftButton == ButtonState.Released)
                {
                    _isMouseDown = false;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            /*if (_isFocused == true)
            {
                if (_isMouseDown == true)
                {
                    spriteBatch.Draw(_mouseDownState.Texture, Position, _mouseDownState.SourceRectangle, Color.White);
                }
                else
                {
                    spriteBatch.Draw(_hoverState.Texture, Position, _hoverState.SourceRectangle, Color.White);
                }
            }
            else
            {*/
            spriteBatch.Draw(_normalState.Texture, Position, _normalState.SourceRectangle, Color.White);
            //}
        }
    }
}
