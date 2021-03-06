﻿#region Header
////////////////////////////////////////////////////////////////////////////// 
//The MIT License (MIT)

//Copyright (c) 2013 Dirk Bretthauer

//Permission is hereby granted, free of charge, to any person obtaining a copy of
//this software and associated documentation files (the "Software"), to deal in
//the Software without restriction, including without limitation the rights to
//use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of
//the Software, and to permit persons to whom the Software is furnished to do so,
//subject to the following conditions:

//The above copyright notice and this permission notice shall be included in all
//copies or substantial portions of the Software.

//THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
//IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS
//FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR
//COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER
//IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN
//CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
///////////////////////////////////////////////////////////////////////////////
#endregion

using System.Collections.Generic;
namespace CChessCore.Pgn
{
    internal class AnnotationState : PgnParserState
    {
        private static Dictionary<string, string> _nags = new Dictionary<string, string>()
            {
                {"0", string.Empty},
                {"1", "!"},
                {"2", "?"},
                {"3", "!!"},
                {"4", "??"},
                {"5", "!?"},
                {"6", "?!"},
                {"7", "(forced move)"},
                {"8", "(singular move)"},
                {"9", "(worst move)"},
            };
        public AnnotationState(PgnParserStatemachine reader)
            : base(reader)
        {
        }

        public override void OnExit()
        {
            var key = GetStateBuffer().Trim();
            if(_nags.ContainsKey(key))
            {
                _currentMove.Annotation = _nags[key];
            }
            else
            {
                _currentMove.Annotation = PgnToken.NumericAnnotationGlyph + key;
            }
        }

        protected override PgnParseResult DoParse(char current, char next, PgnGame currentGame)
        {
            _stateBuffer.Add(current);
                
            if(next == PgnToken.RecursiveVariationEnd.Token)
            {
                OnExit();
                var latest = _history.Pop();
                _statemachine.SetState(latest);
            }
            
            return PgnParseResult.None;
        }
    }
}