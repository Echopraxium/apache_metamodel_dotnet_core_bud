/**
* Licensed to the Apache Software Foundation (ASF) under one
* or more contributor license agreements.  See the NOTICE file
* distributed with this work for additional information
* regarding copyright ownership.  The ASF licenses this file
* to you under the Apache License, Version 2.0 (the
* "License"); you may not use this file except in compliance
* with the License.  You may obtain a copy of the License at
*
*   http://www.apache.org/licenses/LICENSE-2.0
*
* Unless required by applicable law or agreed to in writing,
* software distributed under the License is distributed on an
* "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
* KIND, either express or implied.  See the License for the
* specific language governing permissions and limitations
* under the License.
*/
// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/data/StyleImpl.java
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.j2n.data.numbers;
using org.apache.metamodel.j2n.collections;
using org.apache.metamodel.util;
using System;
using System.Text;

namespace org.apache.metamodel.data
{
    /**
     * Default immutable implementation of {@link Style}.
     */
    public sealed class StyleImpl : BaseObject, Style
    {
        private static readonly long serialVersionUID = 1L;

        private bool           _underline;
        private bool           _italic;
        private bool           _bold;
        private NInteger      _fontSize;
        private TextAlignment  _alignment;
        private Color          _backgroundColor;
        private Color          _foregroundColor;
        private SizeUnit       _fontSizeUnit;

        public StyleImpl() : this(false, false, false, null, SizeUnit.None, TextAlignment.None, null, null)
        {
        } // constructor

        public StyleImpl(bool bold, bool italic, bool underline, NInteger fontSize, SizeUnit fontSizeUnit,
                         TextAlignment alignment, Color backgroundColor, Color foregroundColor)
        {
            _bold = bold;
            _italic = italic;
            _underline = underline;
            _fontSize = fontSize;
            _fontSizeUnit = fontSizeUnit;
            _alignment = alignment;
            _backgroundColor = backgroundColor;
            _foregroundColor = foregroundColor;
        } // constructor

        public bool isBold()
        {
            return _bold;
        } // isBold()

        public bool isItalic()
        {
            return _italic;
        } // isItalic()

        public bool isUnderline()
        {
            return _underline;
        } //  isUnderline()

        public NInteger getFontSize()
        {
            return _fontSize;
        } // getFontSize()

        public SizeUnit getFontSizeUnit()
        {
            return _fontSizeUnit;
        }

        public TextAlignment getAlignment()
        {
            return _alignment;
        }

        public Color getForegroundColor()
        {
            return _foregroundColor;
        }

        public Color getBackgroundColor()
        {
            return _backgroundColor;
        }

        public string toCSS()
        {
            StringBuilder sb = new StringBuilder();
            if (_bold)
            {
                sb.Append("font-weight: bold;");
            }
            if (_italic)
            {
                sb.Append("font-style: italic;");
            }
            if (_underline)
            {
                sb.Append("text-decoration: underline;");
            }
            if (_alignment != TextAlignment.None)
            {
                sb.Append("text-align: " + toCSS(_alignment) + ";");
            }
            if (_fontSize != null)
            {
                sb.Append("font-size: " + _fontSize);
                switch (_fontSizeUnit)
                {
                    case (SizeUnit.PT):
                        sb.Append("pt");
                        break;
                    case (SizeUnit.PX):
                        sb.Append("px");
                        break;
                    case (SizeUnit.PERCENT):
                        sb.Append("%");
                        break;
                    default:
                        break;
                }
                sb.Append(';');
            }
            if (_foregroundColor != null)
            {
                sb.Append("color: " + toCSS(_foregroundColor) + ";");
            }
            if (_backgroundColor != null)
            {
                sb.Append("background-color: " + toCSS(_backgroundColor) + ";");
            }
            return sb.ToString();
        }

        private string toCSS(Color c)
        {
            return "rgb(" + c.getRed() + "," + c.getGreen() + "," + c.getBlue() + ")";
        }

        public override string ToString()
        {
            return toCSS();
        } // ToString()

        private string toCSS(TextAlignment alignment)
        {
            switch (alignment)
            {
                case TextAlignment.LEFT:
                    return "left";
                case TextAlignment.RIGHT:
                    return "right";
                case TextAlignment.CENTER:
                    return "center";
                case TextAlignment.JUSTIFY:
                    return "justify";
                default:
                    throw new InvalidOperationException("Unknown alignment: " + alignment);
            }
        }

        protected override void decorateIdentity( NList<object> identifiers)
        {
            identifiers.Add(_underline);
            identifiers.Add(_italic);
            identifiers.Add(_bold);
            identifiers.Add(_fontSize);
            identifiers.Add(_fontSizeUnit);
            identifiers.Add(_alignment);
            identifiers.Add(_backgroundColor);
            identifiers.Add(_foregroundColor);
        } // decorateIdentity()
    } // StyleImpl class
} // org.apache.metamodel.data naùespace
