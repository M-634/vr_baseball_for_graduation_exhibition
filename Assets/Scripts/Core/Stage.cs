using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// �X�e�[�W�N���X.
/// </summary>
[Serializable]
public class Stage 
{
    /// <summary>�X�e�[�W�i���o�[</summary>
    public int stageNumber;
    /// <summary>�X�y�V�����X�e�[�W�i�ŏI�X�e�[�W�j���ǂ������肷��t���O</summary>
    public bool specialStage;
    /// <summary>�s�b�`���[�������Ă��鋅��</summary>
    public BallType[] ballTypes;
    /// <summary>�s�b�`���[�������Ă��鋅��</summary>
    public int ballNumber;
    /// <summary>�X�e�[�W�N���A�ɕK�v�ȑœ_��</summary>
    public int clearHitNumer;
}

